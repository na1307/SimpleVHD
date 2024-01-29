using Microsoft.Win32;
using SimpleVhd.Bcd;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using static SimpleVhd.BcdEdit;

namespace SimpleVhd.Installer;

public sealed class NewInstallProcessor : InstallProcessor {
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
    }

    public override void InstallProcess() {
        Match m = BcdEditRegex("/enum {current} /v", @"^device\s+vhd=\[(?<drv>[A-Z]:)\](?<path>.+\.vhdx?)");
        VhdDrive = m.Groups["drv"].Value;
        VhdPath = Path.GetDirectoryName(m.Groups["path"].Value)!;

        if (VhdPath.Length > 1) {
            VhdPath += Path.DirectorySeparatorChar.ToString();
        }

        VhdName = Path.GetFileNameWithoutExtension(m.Groups["path"].Value);
        Format = Enum.Parse<VhdFormat>(Regex.Match(Path.GetFileName(m.Groups["path"].Value), "^.+\\.(?<ext>vhdx?)$", RegexOptions.IgnoreCase).Groups["ext"].Value, true);

        Directory.CreateDirectory(Path.Combine(SVDir, BackupDirName));
        Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", "SimpleVHD Startup", Path.Combine(SVDir, "Bin", "Startup.exe"), RegistryValueKind.String);

        string arch;

        if (Status.IsX64) {
            arch = "x64";
        } else if (Status.IsArm64) {
            arch = "arm64";
        } else {
            throw new InvalidOperationException();
        }

        BcdObject parrent = BcdStore.SystemStore.OpenObject(WellKnownGuids.Current);

        BcdObject child1 = BcdStore.SystemStore.CopyObject(parrent);
        child1.SetVhdDeviceElement(BcdElementType.BcdLibraryApplicationDevice, $"{VhdPath}{Child1Name}.{Format.ToString().ToLower()}", DeviceType.PartitionDevice, null, DevicePath.GetDevicePath(VhdDrive), 0);
        child1.SetVhdDeviceElement(BcdElementType.BcdOSLoaderOSDevice, $"{VhdPath}{Child1Name}.{Format.ToString().ToLower()}", DeviceType.PartitionDevice, null, DevicePath.GetDevicePath(VhdDrive), 0);

        BcdObject child2 = BcdStore.SystemStore.CopyObject(parrent);
        child2.SetVhdDeviceElement(BcdElementType.BcdLibraryApplicationDevice, $"{VhdPath}{Child2Name}.{Format.ToString().ToLower()}", DeviceType.PartitionDevice, null, DevicePath.GetDevicePath(VhdDrive), 0);
        child2.SetVhdDeviceElement(BcdElementType.BcdOSLoaderOSDevice, $"{VhdPath}{Child2Name}.{Format.ToString().ToLower()}", DeviceType.PartitionDevice, null, DevicePath.GetDevicePath(VhdDrive), 0);

        BcdObject ramdisk = BcdStore.SystemStore.CreateObject(Guid.NewGuid(), BcdObjectType.Device);
        ramdisk.SetPartitionDeviceElement(BcdElementType.BcdDeviceSdiDevice, DeviceType.PartitionDevice, null, DevicePath.GetDevicePath(SVDrive));
        ramdisk.SetStringElement(BcdElementType.BcdDeviceSdiPath, SVPath + "Boot\\boot.sdi");

        BcdObject pe = BcdStore.SystemStore.CreateObject(Guid.NewGuid(), BcdObjectType.BootLoader);
        pe.SetStringElement(BcdElementType.BcdLibraryDescription, "SimpleVHD PE");
        pe.SetFileDeviceElement(BcdElementType.BcdLibraryApplicationDevice, DeviceType.RamdiskDevice, ramdisk, $"{SVPath}Boot\\{arch}.wim", DeviceType.PartitionDevice, null, DevicePath.GetDevicePath(SVDrive));
        pe.SetStringElement(BcdElementType.BcdLibraryApplicationPath, $@"\windows\system32\winload.{(Firmware.IsWindowsUEFI ? "efi" : "exe")}");
        pe.SetObjectListElement(BcdElementType.BcdLibraryInheritedObjects, BcdStore.SystemStore.OpenObject(WellKnownGuids.BootLoaderSettings));
        pe.SetFileDeviceElement(BcdElementType.BcdOSLoaderOSDevice, DeviceType.RamdiskDevice, ramdisk, $"{SVPath}Boot\\{arch}.wim", DeviceType.PartitionDevice, null, DevicePath.GetDevicePath(SVDrive));
        pe.SetStringElement(BcdElementType.BcdOSLoaderSystemRoot, "\\windows");
        pe.SetBooleanElement(BcdElementType.BcdOSLoaderDetectKernelAndHal, true);
        pe.SetBooleanElement(BcdElementType.BcdOSLoaderWinPEMode, true);

        BcdObject bootmgr = BcdStore.SystemStore.OpenObject(WellKnownGuids.BootMgr);
        bootmgr.SetIntegerElement(BcdElementType.BcdBootMgrTimeout, 5);
        bootmgr.SetObjectListElement(BcdElementType.BcdBootMgrDisplayOrder, [.. ((BcdObject[])bootmgr.GetElement(BcdElementType.BcdBootMgrDisplayOrder)), pe]);

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
                { "ParentGuid", parrent.Id.ToString("B") },
                { "Child1Guid", child1.Id.ToString("B") },
                { "Child2Guid", child2.Id.ToString("B") }
            },
                ])
            },
            { "RamdiskGuid", ramdisk.Id.ToString("B") },
            { "PEGuid", pe.Id.ToString("B") },
        }.WriteTo(writer);
    }
}
