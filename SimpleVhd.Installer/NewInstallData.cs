using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using static SimpleVhd.BcdEdit;

namespace SimpleVhd.Installer;

public sealed class NewInstallData : InstallData {
    public override void CheckRequirements() {
        if (!Environment.Is64BitOperatingSystem) {
            throw new RequirementsNotMetException("64비트 운영 체제만 지원합니다.");
        }

        IEnumerable<string> drvs = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed && Directory.Exists(Path.Combine(d.Name, DirName))).Select(d => d.Name.Split("\\")[0]);

        if (drvs.Any()) {
            SVDrive = drvs.First();
            SVPath = Path.DirectorySeparatorChar.ToString() + DirName + Path.DirectorySeparatorChar.ToString();
            SVDir = SVDrive + SVPath;
        } else {
            throw new RequirementsNotMetException("아무 드라이브의 루트에 " + DirName + " 폴더가 없습니다.");
        }

        IEnumerable<string> requires = ((string[])["Boot\\x64.wim", "Boot\\arm64.wim", "Boot\\boot.sdi"]).Where(f => !File.Exists(Path.Combine(SVDir, f)));

        if (requires.Any()) {
            throw new RequirementsNotMetException(Path.GetFileName(requires.First()) + " 파일이 없습니다.");
        }

        Match m = BcdEditRegex("/enum {current} /v", @"^device\s+vhd=\[(?<drv>[A-Z]:)\](?<path>.+\.vhdx?)");
        VhdDrive = m.Groups["drv"].Value;
        VhdPath = Path.GetDirectoryName(m.Groups["path"].Value)!;

        if (VhdPath.Length > 1) {
            VhdPath += Path.DirectorySeparatorChar.ToString();
        }

        VhdName = Path.GetFileNameWithoutExtension(m.Groups["path"].Value);
        Format = Enum.Parse<VhdFormat>(Regex.Match(Path.GetFileName(m.Groups["path"].Value), "^.+\\.(?<ext>vhdx?)$", RegexOptions.IgnoreCase).Groups["ext"].Value, true);
    }

    public override void InstallProcess() {
        var bcdDesctiption = BcdEditRegex("/enum {current}", @"^description\s+(?<name>.+)$").Groups["name"].Value;

        Guid parent, child1, child2, ramdisk, pe;
        string arch;

        if (Statics.IsX64) {
            arch = "x64";
        } else if (Statics.IsArm64) {
            arch = "arm64";
        } else {
            throw new InvalidOperationException();
        }

        parent = BcdEditGuid($"/enum {{current}} /v");

        child1 = BcdEditGuid($"/copy {{current}} /d \"{bcdDesctiption}\"");
        ProcessBcdEdit($@"/set {child1:B} device vhd=""[{VhdDrive}]{VhdPath}{Child1Name}.{Format},locate=custom:12000002");
        ProcessBcdEdit($@"/set {child1:B} osdevice vhd=""[{VhdDrive}]{VhdPath}{Child1Name}.{Format},locate=custom:22000002");
        ProcessBcdEdit($@"/displayorder {child1:B} /remove");

        child2 = BcdEditGuid($"/copy {{current}} /d \"{bcdDesctiption}\"");
        ProcessBcdEdit($@"/set {child2:B} device vhd=""[{VhdDrive}]{VhdPath}{Child2Name}.{Format},locate=custom:12000002");
        ProcessBcdEdit($@"/set {child2:B} osdevice vhd=""[{VhdDrive}]{VhdPath}{Child2Name}.{Format},locate=custom:22000002");
        ProcessBcdEdit($@"/displayorder {child2:B} /remove");

        ramdisk = BcdEditGuid($"/create /device");
        ProcessBcdEdit($@"/set {ramdisk:B} ramdisksdidevice partition={SVDrive}");
        ProcessBcdEdit($@"/set {ramdisk:B} ramdisksdipath {SVPath}Boot\boot.sdi");

        pe = BcdEditGuid($"/create /d \"SimpleVHD PE\" /application OSLOADER");
        ProcessBcdEdit($@"/set {pe:B} device ramdisk=""[{SVDrive}]{SVPath}Boot\{arch}.wim,{ramdisk:B}""");
        ProcessBcdEdit($@"/set {pe:B} path \windows\system32\winload.{(Firmware.IsWindowsUEFI ? "efi" : "exe")}");
        ProcessBcdEdit($@"/set {pe:B} inherit {{bootloadersettings}}");
        ProcessBcdEdit($@"/set {pe:B} osdevice ramdisk=""[{SVDrive}]{SVPath}Boot\{arch}.wim,{ramdisk:B}""");
        ProcessBcdEdit($@"/set {pe:B} systemroot \windows");
        ProcessBcdEdit($@"/set {pe:B} detecthal yes");
        ProcessBcdEdit($@"/set {pe:B} winpe yes");
        ProcessBcdEdit($@"/displayorder {pe:B} /addlast");

        ProcessBcdEdit($@"/timeout 5");

        using FileStream fs = new(Path.Combine(Application.StartupPath, "..", SettingsFileName), FileMode.Create, FileAccess.Write, FileShare.None);
        using Utf8JsonWriter writer = new(fs, new JsonWriterOptions() { Indented = true });

        new JsonObject() {
            {
                "VhdInstances",
                (JsonArray)([
            new JsonObject() {
                { "Directory", VhdPath },
                { "ParentFile", VhdName },
                { "Style", Style.Normal.ToString() },
                { nameof(Type), Type.ToString() },
                { nameof(Format), Format.ToString() },
                { "ParentGuid", parent.ToString("B") },
                { "Child1Guid", child1.ToString("B") },
                { "Child2Guid", child2.ToString("B") }
            },
                ])
            },
            { "RamdiskGuid", ramdisk.ToString("B") },
            { "PEGuid", pe.ToString("B") },
        }.WriteTo(writer);

        static Guid BcdEditGuid(string arg) => Guid.Parse(BcdEditRegex(arg, @"(?<guid>\{.+\})").Groups["guid"].Value);
    }
}