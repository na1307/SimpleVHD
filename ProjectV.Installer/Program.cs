using Microsoft.Win32;
using ProjectV;
using ProjectV.Installer;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static ProjectV.BcdEdit;
using static System.Console;

const string line = "==================================================";

try {
    Title = "Project V 설치";
    WindowHeight = 24;
    BackgroundColor = ConsoleColor.DarkBlue;
    ForegroundColor = ConsoleColor.White;
    Clear();

    var winVer = Environment.OSVersion.Version.Major switch {
        6 => Environment.OSVersion.Version.Minor switch {
            1 => WinVer.Seven,
            2 => WinVer.Eight,
            3 => WinVer.Epnt1,
            4 => WinVer.Ten,
            _ => throw new PlatformNotSupportedException(),
        },
        10 => WinVer.Ten,
        _ => throw new PlatformNotSupportedException(),
    };

    string pvDir = string.Empty;
    string pvDrv = string.Empty;
    string pvPath = string.Empty;

    foreach (var drv in from d in DriveInfo.GetDrives()
                        where d.CheckFixed() && Directory.Exists(d.Name + DirName)
                        select d.GetLetter()) {
        pvDir = drv + "\\" + DirName + "\\";
        pvDrv = drv;
        pvPath = "\\" + DirName + "\\";
        break;
    }

    if (string.IsNullOrEmpty(pvDir)) throw new RequirementNotFoundException();

    foreach (var file in from f in new[] { "Boot\\ProjectV.wim", "Boot\\boot.sdi" } where !File.Exists(pvDir + f) select f) throw new RequirementNotFoundException(Path.GetFileName(file));

    if (File.Exists(pvDir + "\\" + ConfigName) && !ConfigExists()) return;

    string vhdDrv, vhdPath, vhdName;

    try {
        var m = BcdEditRegex("/enum {current} /v", @"^device\s+vhd=\[(?<drv>[A-Z]:)\](?<path>.+\.vhdx?)");

        vhdDrv = m.Groups["drv"].Value;
        vhdPath = Path.GetDirectoryName(m.Groups["path"].Value);
        if (vhdPath.Length > 1) vhdPath += "\\";
        vhdName = Path.GetFileName(m.Groups["path"].Value);
    } catch (BcdException ex) {
        throw new VhdNotFoundException(ex);
    }

    var vhdType = GetVhdType();
    var vhdFormat = Regex.Match(vhdName, "^.+\\.(?<ext>vhdx?)$", RegexOptions.IgnoreCase).Groups["ext"].Value.ToLower();
    var operatingStyle = GetOperatingStyle();

    Directory.CreateDirectory(GetBackupDrive() + "\\" + BackupDirName);

    using (StreamWriter vbs = new(pvDir + "Agent.vbs", false, System.Text.Encoding.GetEncoding(949))) {
        vbs.WriteLine(
$@"Set WshShell = CreateObject(""WScript.Shell"")
WshShell.Run """"""{pvDir}Bin\Agent.exe"""""", 0
Set WshShell = Nothing"
);
    }

    Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", "PV_Agent", pvDir + "Agent.vbs", RegistryValueKind.String);

    BackupBcd(pvDir, 1);

    var bcdDesctiption = BcdEditRegex("/enum {current}", @"^description\s+(?<name>.+)$").Groups["name"].Value;

    Dictionary<string, string> guids = new(5);

    guids.Add("Parent", BcdEditGuid("/enum {current} /v"));

    guids.Add("Child1", BcdEditGuid("/copy {current} /d \"" + bcdDesctiption + "\""));

    ProcessBcdEdit($@"/set {guids["Child1"]} device vhd=""[{vhdDrv}]{vhdPath}{Child1Name}{vhdFormat}");
    ProcessBcdEdit($@"/set {guids["Child1"]} osdevice vhd=""[{vhdDrv}]{vhdPath}{Child1Name}{vhdFormat}");
    ProcessBcdEdit($@"/displayorder {guids["Child1"]} /remove");

    guids.Add("Child2", BcdEditGuid("/copy {current} /d \"" + bcdDesctiption + "\""));

    ProcessBcdEdit($@"/set {guids["Child2"]} device vhd=""[{vhdDrv}]{vhdPath}{Child2Name}{vhdFormat}");
    ProcessBcdEdit($@"/set {guids["Child2"]} osdevice vhd=""[{vhdDrv}]{vhdPath}{Child2Name}{vhdFormat}");
    ProcessBcdEdit($@"/displayorder {guids["Child2"]} /remove");

    guids.Add("Ramdisk", BcdEditGuid("/create /device"));

    ProcessBcdEdit($@"/set {guids["Ramdisk"]} ramdisksdidevice partition={pvDrv}");
    ProcessBcdEdit($@"/set {guids["Ramdisk"]} ramdisksdipath {pvPath}Boot\boot.sdi");

    guids.Add("Processor", BcdEditGuid("/create /d \"Project V Processor\" /application OSLOADER"));

    ProcessBcdEdit($@"/set {guids["Processor"]} device ramdisk=""[{pvDrv}]{pvPath}Boot\ProjectV.wim,{guids["Ramdisk"]}""");
    ProcessBcdEdit($@"/set {guids["Processor"]} path \windows\system32\winload.{(Firmware.IsWindowsUEFI ? "efi" : "exe")}");
    ProcessBcdEdit($@"/set {guids["Processor"]} inherit {{bootloadersettings}}");
    ProcessBcdEdit($@"/set {guids["Processor"]} osdevice ramdisk=""[{pvDrv}]{pvPath}Boot\ProjectV.wim,{guids["Ramdisk"]}""");
    ProcessBcdEdit($@"/set {guids["Processor"]} systemroot \windows");
    ProcessBcdEdit($@"/set {guids["Processor"]} detecthal yes");
    ProcessBcdEdit($@"/set {guids["Processor"]} winpe yes");
    ProcessBcdEdit($@"/displayorder {guids["Processor"]} /addlast");

    ProcessBcdEdit($@"/timeout 5");
    ProcessBcdEdit($@"/bootsequence {guids["Processor"]}");

    BackupBcd(pvDir, 2);

    new XDocument(
        new XComment(ConfigComment),
        new XElement(
            "Config",
            new XElement("WinVer", winVer.ToString()),
            new XElement("OperatingStyle", OperatingStyle.Simple.ToString()),
            new XElement("VhdType", vhdType.ToString()),
            new XElement("VhdFormat", ((VhdFormat)Enum.Parse(typeof(VhdFormat), vhdFormat.ToUpper(), true)).ToString()),
            new XElement("VhdDirectory", vhdPath),
            new XElement("VhdFile", vhdName),
            new XElement("Action", DoAction.DoSwitchStyle.ToString()),
            GenerateXEs(guids),
            new XElement("Temp", operatingStyle.ToString())
        )
    ).Save(pvDir + "\\" + ConfigName);

    using (Process shutdown = new() {
        StartInfo = new() {
            FileName = "shutdown.exe",
            Arguments = "/r /t 0",
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Hidden
        }
    }) {
        shutdown.Start();
    }
} catch (PlatformNotSupportedException) {
    ErrorControl("이 운영 체제는 지원되지 않습니다.");
} catch (PVInstallerException ex) {
    ErrorControl(ex.Message);
} catch (Exception ex) {
    ErrorControl(ex.ToString());
}

bool ConfigExists() {
    while (true) {
        Clear();
        Beep();
        Write("Project V가 이미 설치된 것 같습니다. 계속하시겠습니까?\r\n\r\n Y or N) : ");

        switch (ReadLine().ToUpper()) {
            case "Y": return true;
            case "N": return false;
            default: continue;
        }
    }
}

VhdType GetVhdType() {
    while (true) {
        Clear();
        Beep();
        Write(
$@"
현재 VHD의 유형을 선택하세요.

{line}
 1. 동적 확장 [Expandable]

 2. 고정 크기 [Fixed]
{line}

 1 or 2) : "
 );

        switch (ReadLine()) {
            case "1": return VhdType.Expandable;
            case "2": return VhdType.Fixed;
            default: continue;
        }
    }
}

OperatingStyle GetOperatingStyle() {
    while (true) {
        Clear();
        Beep();
        Write(
$@"
설치 후 기본적으로 사용할 VHD 운영 스타일을 선택하세요.

{line}
 1. 단순 스타일                  [원본 백업, 복원]

 2. 차등 스타일 (수동 초기화)      [변경분 초기화]

 3. 차등 스타일 (자동 초기화) [변경분 자동 초기화]
{line}

 1 ~ 3) : "
 );

        switch (ReadLine()) {
            case "1": return OperatingStyle.Simple;
            case "2": return OperatingStyle.DifferentialManual;
            case "3": return OperatingStyle.DifferentialAuto;
            default: continue;
        }
    }
}

string GetBackupDrive() {
    while (true) {
        Clear();
        Beep();

        WriteLine("\r\n아래의 목록을 보고 백업을 저장할 드라이브를 입력하세요.\r\n\r\n" + line + "\r\n 문자  레이블      Fs    크기\r\n" + (new string('-', 50)));

        foreach (var drvinfo in DriveInfo.GetDrives().Where(ProjectV.Extensions.CheckFixed)) {
            WriteLine(" " + drvinfo.Name[0] + "     " + drvinfo.VolumeLabel + "      " + drvinfo.DriveFormat + "    " + (drvinfo.TotalSize / 1024 / 1024 / 1024) + " GB");
        }

        Write(line + "\r\n\r\n ex) D or D:");

        var input = ReadLine();

        if (Regex.IsMatch(input, "^[A-Z]:?$", RegexOptions.IgnoreCase) && (new DriveInfo(input).DriveType == DriveType.Fixed)) return input[0] + ":";
    }
}

void BackupBcd(string path, int i) => ProcessBcdEdit("/export \"" + path + "Backup-BCD-0" + i + "\"");

string BcdEditGuid(string arg) => BcdEditRegex(arg, @"(?<guid>\{.+\})").Groups["guid"].Value;

IEnumerable<XElement> GenerateXEs(Dictionary<string, string> keyValuePairs) => ShutdownAction.Select(e => new XElement("ShutdownAfterAction", new XAttribute("Type", e.ToString().Substring(2)), false)).Concat(keyValuePairs.Select(p => new XElement("Guid", new XAttribute("Type", p.Key), p.Value)));

void ErrorControl(string message) {
    BackgroundColor = ConsoleColor.DarkRed;
    ForegroundColor = ConsoleColor.White;
    Clear();
    WriteLine(line + "\r\n" + message + "\r\n" + line);
    Beep();
    Beep();
    Beep();
    Write("\r\n종료하려면 아무 키나 누르시오... ");
    ReadKey(true);
    Environment.Exit(1);
}