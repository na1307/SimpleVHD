using Microsoft.Win32;
using SimpleVhd.Bcd;
using System.Management;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using static SimpleVhd.DevicePathMapper;

namespace SimpleVhd.Installer;

public sealed class NewInstallProcessor : InstallProcessor {
    public override void InstallProcess() {
        var vp = getVhdPath(getSystemDiskNumber());
        VhdDrive = Path.GetPathRoot(vp)!.TrimEnd('\\');
        VhdPath = Path.GetDirectoryName(vp[2..])!;

        if (VhdPath.Length > 1) {
            VhdPath += Path.DirectorySeparatorChar.ToString();
        }

        VhdName = Path.GetFileNameWithoutExtension(vp);
        Format = Enum.Parse<VhdFormat>(Path.GetExtension(vp)[1..], true);

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
        child1.SetVhdDeviceElement(BcdElementType.BcdLibraryApplicationDevice, $"{VhdPath}{Child1Name}.{Format.ToString().ToLower()}", DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);
        child1.SetVhdDeviceElement(BcdElementType.BcdOSLoaderOSDevice, $"{VhdPath}{Child1Name}.{Format.ToString().ToLower()}", DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);

        BcdObject child2 = BcdStore.SystemStore.CopyObject(parrent);
        child2.SetVhdDeviceElement(BcdElementType.BcdLibraryApplicationDevice, $"{VhdPath}{Child2Name}.{Format.ToString().ToLower()}", DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);
        child2.SetVhdDeviceElement(BcdElementType.BcdOSLoaderOSDevice, $"{VhdPath}{Child2Name}.{Format.ToString().ToLower()}", DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);

        BcdObject ramdisk = BcdStore.SystemStore.CreateObject(Guid.NewGuid(), BcdObjectType.Device);
        ramdisk.SetPartitionDeviceElement(BcdElementType.BcdDeviceSdiDevice, DeviceType.PartitionDevice, null, GetDevicePath(SVDrive));
        ramdisk.SetStringElement(BcdElementType.BcdDeviceSdiPath, SVPath + "Boot\\boot.sdi");

        BcdObject pe = BcdStore.SystemStore.CreateObject(Guid.NewGuid(), BcdObjectType.BootLoader);
        pe.SetStringElement(BcdElementType.BcdLibraryDescription, "SimpleVHD PE");
        pe.SetFileDeviceElement(BcdElementType.BcdLibraryApplicationDevice, DeviceType.RamdiskDevice, ramdisk, $"{SVPath}Boot\\{arch}.wim", DeviceType.PartitionDevice, null, GetDevicePath(SVDrive));
        pe.SetStringElement(BcdElementType.BcdLibraryApplicationPath, $@"\windows\system32\winload.{(Firmware.IsWindowsUEFI ? "efi" : "exe")}");
        pe.SetObjectListElement(BcdElementType.BcdLibraryInheritedObjects, BcdStore.SystemStore.OpenObject(WellKnownGuids.BootLoaderSettings));
        pe.SetFileDeviceElement(BcdElementType.BcdOSLoaderOSDevice, DeviceType.RamdiskDevice, ramdisk, $"{SVPath}Boot\\{arch}.wim", DeviceType.PartitionDevice, null, GetDevicePath(SVDrive));
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

    private static string getVhdPath(int number) {
        ManagementBaseObject queryObj = new ManagementObjectSearcher(@"root\Microsoft\Windows\Storage", "SELECT * FROM MSFT_PhysicalDisk WHERE DeviceID=\"" + number.ToString() + "\"").Get().Cast<ManagementBaseObject>().First();
        var pl = queryObj["PhysicalLocation"].ToString();

        if (pl![..22] is not @"\Device\HarddiskVolume") {
            throw new SimpleVhdException("부팅된 가상 디스크가 아닙니다.");
        }

        return FromDevicePath(pl);
    }

    private static int getSystemDiskNumber() {
        const string q = "SELECT * FROM Win32_LogicalDiskToPartition";
        var a = new ManagementObjectSearcher(q).Get().Cast<ManagementObject>().Where(drive => ((string)drive["Dependent"]).Contains("C:")).Select(drive => (string)drive["Antecedent"]).First();

        return int.Parse(Regex.Match(a, "Disk #(?<number>[0-9]+), Partition #[0-9]+").Groups["number"].Value);
    }
}
