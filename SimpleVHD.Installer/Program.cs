using Microsoft.Win32;
using SimpleVHD;
using SimpleVHD.Installer;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static SimpleVHD.BcdEdit;
using static System.Console;

const string line = "==================================================";

try {
    Title = "SimpleVHD 설치";
    WindowHeight = 24;
    BackgroundColor = ConsoleColor.DarkBlue;
    ForegroundColor = ConsoleColor.White;
    Clear();

    string pvDir, pvDrv, pvPath;

    var drvs = from d in DriveInfo.GetDrives()
               where d.CheckFixed() && Directory.Exists(d.Name + DirName)
               select d.GetLetter();

    if (drvs.Any()) {
        pvDrv = drvs.First();
        pvPath = Path.DirectorySeparatorChar.ToString() + DirName + Path.DirectorySeparatorChar.ToString();
        pvDir = pvDrv + pvPath;
    } else {
        throw new RequirementNotFoundException();
    }

    var requires = new[] { "Boot\\SimpleVHD.wim", "Boot\\boot.sdi" }.Where(f => !File.Exists(pvDir + f));

    if (requires.Any()) throw new RequirementNotFoundException(Path.GetFileName(requires.First()));

    if (File.Exists(pvDir + Path.DirectorySeparatorChar.ToString() + ConfigName) && !ConfigExists()) return;

    string vhdDrv, vhdPath, vhdName;

    try {
        var m = BcdEditRegex("/enum {current} /v", @"^device\s+vhd=\[(?<drv>[A-Z]:)\](?<path>.+\.vhdx?)");

        vhdDrv = m.Groups["drv"].Value;
        vhdPath = Path.GetDirectoryName(m.Groups["path"].Value);
        if (vhdPath.Length > 1) vhdPath += Path.DirectorySeparatorChar.ToString();
        vhdName = Path.GetFileName(m.Groups["path"].Value);
    } catch (BcdException ex) {
        throw new VhdNotFoundException(ex);
    }

    var vhdType = GetVhdType();
    var vhdFormat = Regex.Match(vhdName, "^.+\\.(?<ext>vhdx?)$", RegexOptions.IgnoreCase).Groups["ext"].Value.ToLower();
    var operatingStyle = GetOperatingStyle();
    var backDrv = GetBackupDrive();

    Directory.CreateDirectory(backDrv.Equals(pvDrv, StringComparison.OrdinalIgnoreCase) ? pvDir + IncludedBackupDirName : backDrv + Path.DirectorySeparatorChar.ToString() + BackupDirName);

    Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", "PVStartup", pvDir + "Bin\\Startup.exe", RegistryValueKind.String);

    BackupBcd(pvDir, 1);

    var bcdDesctiption = BcdEditRegex("/enum {current}", @"^description\s+(?<name>.+)$").Groups["name"].Value;

#pragma warning disable IDE0028 // Simplify collection initialization
    Dictionary<string, string> guids = new(5);
#pragma warning restore IDE0028 // Simplify collection initialization

    guids.Add("Parent", BcdEditGuid($"/enum {{current}} /v"));

    guids.Add("Child1", BcdEditGuid($"/copy {{current}} /d \"{bcdDesctiption}\""));

    ProcessBcdEdit($@"/set {guids["Child1"]} device vhd=""[{vhdDrv}]{vhdPath}{Child1Name}{vhdFormat}");
    ProcessBcdEdit($@"/set {guids["Child1"]} osdevice vhd=""[{vhdDrv}]{vhdPath}{Child1Name}{vhdFormat}");
    ProcessBcdEdit($@"/displayorder {guids["Child1"]} /remove");

    guids.Add("Child2", BcdEditGuid($"/copy {{current}} /d \"{bcdDesctiption}\""));

    ProcessBcdEdit($@"/set {guids["Child2"]} device vhd=""[{vhdDrv}]{vhdPath}{Child2Name}{vhdFormat}");
    ProcessBcdEdit($@"/set {guids["Child2"]} osdevice vhd=""[{vhdDrv}]{vhdPath}{Child2Name}{vhdFormat}");
    ProcessBcdEdit($@"/displayorder {guids["Child2"]} /remove");

    guids.Add("Ramdisk", BcdEditGuid($"/create /device"));

    ProcessBcdEdit($@"/set {guids["Ramdisk"]} ramdisksdidevice partition={pvDrv}");
    ProcessBcdEdit($@"/set {guids["Ramdisk"]} ramdisksdipath {pvPath}Boot\boot.sdi");

    guids.Add("PE", BcdEditGuid($"/create /d \"SimpleVHD PE Action\" /application OSLOADER"));

    ProcessBcdEdit($@"/set {guids["PE"]} device ramdisk=""[{pvDrv}]{pvPath}Boot\SimpleVHD.wim,{guids["Ramdisk"]}""");
    ProcessBcdEdit($@"/set {guids["PE"]} path \windows\system32\winload.{(Firmware.IsWindowsUEFI ? "efi" : "exe")}");
    ProcessBcdEdit($@"/set {guids["PE"]} inherit {{bootloadersettings}}");
    ProcessBcdEdit($@"/set {guids["PE"]} osdevice ramdisk=""[{pvDrv}]{pvPath}Boot\SimpleVHD.wim,{guids["Ramdisk"]}""");
    ProcessBcdEdit($@"/set {guids["PE"]} systemroot \windows");
    ProcessBcdEdit($@"/set {guids["PE"]} detecthal yes");
    ProcessBcdEdit($@"/set {guids["PE"]} winpe yes");
    ProcessBcdEdit($@"/displayorder {guids["PE"]} /addlast");

    ProcessBcdEdit($@"/timeout 5");
    ProcessBcdEdit($@"/bootsequence {guids["PE"]}");

    BackupBcd(pvDir, 2);

    new XDocument(
        new XComment(ConfigComment),
        new XElement(
            "Config",
            new XElement("WindowsVersion", Winver.WindowsVersion.ToString()),
            new XElement("OperatingStyle", OperatingStyle.Simple.ToString()),
            new XElement("VhdType", vhdType.ToString()),
            new XElement("VhdFormat", EnumParser.Parse<VhdFormat>(vhdFormat, true).ToString()),
            new XElement("VhdDirectory", vhdPath),
            new XElement("VhdFile", vhdName),
            new XElement("Action", DoAction.DoSwitchStyle.ToString()),
            GenerateXEs(guids),
            new XElement("Temp", operatingStyle.ToString())
        )
    ).Save(pvDir + Path.DirectorySeparatorChar.ToString() + ConfigName);

#pragma warning disable IDE0063 // Use simple 'using' statement
    using (Process shutdown = new() {
        StartInfo = {
            FileName = "shutdown.exe",
            Arguments = "/r /t 0",
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Hidden
        }
    }) {
        shutdown.Start();
    }
#pragma warning restore IDE0063 // Use simple 'using' statement
} catch (PVInstallerException ex) {
    ErrorControl(ex.Message);
} catch (Exception ex) {
    ErrorControl(ex.ToString());
}

static bool ConfigExists() {
    while (true) {
        Clear();
        Beep();
        Write("SimpleVHD가 이미 설치된 것 같습니다. 계속하시겠습니까?\r\n\r\n Y or N) : ");

        switch (ReadLine().ToUpper()) {
            case "Y": return true;
            case "N": return false;
            default: continue;
        }
    }
}

static VhdType GetVhdType() {
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

static OperatingStyle GetOperatingStyle() {
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

static string GetBackupDrive() {
    while (true) {
        Clear();
        Beep();

        WriteLine("\r\n아래의 목록을 보고 백업을 저장할 드라이브를 입력하세요.\r\n\r\n" + line + "\r\n 문자  레이블      Fs    크기\r\n" + (new string('-', 50)));
        DriveInfo.GetDrives().Where(Extensions.CheckFixed).Select(d => " " + d.Name[0] + "     " + d.VolumeLabel + "      " + d.DriveFormat + "    " + (d.TotalSize / 1024 / 1024 / 1024) + " GB").ForEach(WriteLine);
        Write(line + "\r\n\r\n ex) D or D:");

        var input = ReadLine();

        if (Regex.IsMatch(input, "^[A-Z]:?$", RegexOptions.IgnoreCase) && new DriveInfo(input).CheckFixed()) return input[0] + ":";
    }
}

static void BackupBcd(string path, int i) => ProcessBcdEdit("/export \"" + path + "Backup-BCD-0" + i + "\"");

static string BcdEditGuid(string arg) => BcdEditRegex(arg, @"(?<guid>\{.+\})").Groups["guid"].Value;

static IEnumerable<XElement> GenerateXEs(Dictionary<string, string> keyValuePairs) => ShutdownAction.Select(e => new XElement("ShutdownAfterAction", new XAttribute("Type", e.ToString().Substring(2)), false)).Concat(keyValuePairs.Select(p => new XElement("Guid", new XAttribute("Type", p.Key), p.Value)));

static void ErrorControl(string message) {
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