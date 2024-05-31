using Bluehill.Bcd;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SimpleVhd.Installer;

public sealed class NewInstallProcessor : InstallProcessor {
    public override void InstallProcess() {
        Directory.CreateDirectory(Path.Combine(SVPath, BackupDirName));

        var arch = RuntimeInformation.ProcessArchitecture switch {
            Architecture.X64 => "x64",
            Architecture.Arm64 => "arm64",
            _ => throw new NotImplementedException(),
        };

        var parent = BcdStore.SystemStore.OpenObject(WellKnownGuids.Current);
        var driveDP = GetDevicePath(VhdDrive);

        var child1 = BcdStore.SystemStore.CopyObject(parent, CopyObjectOptions.CreateNewId);
        var child1Path = Path.Combine(VhdPath, $"{VhdFileName}-{Child1FileName}.{VhdFormat.ToString().ToLower()}");
        child1.SetVhdDeviceElement(BcdElementType.BcdLibraryApplicationDevice, child1Path, DeviceType.PartitionDevice, null, driveDP, 0);
        child1.SetVhdDeviceElement(BcdElementType.BcdOSLoaderOSDevice, child1Path, DeviceType.PartitionDevice, null, driveDP, 0);

        var child2 = BcdStore.SystemStore.CopyObject(parent, CopyObjectOptions.CreateNewId);
        var child2Path = Path.Combine(VhdPath, $"{VhdFileName}-{Child2FileName}.{VhdFormat.ToString().ToLower()}");
        child2.SetVhdDeviceElement(BcdElementType.BcdLibraryApplicationDevice, child2Path, DeviceType.PartitionDevice, null, driveDP, 0);
        child2.SetVhdDeviceElement(BcdElementType.BcdOSLoaderOSDevice, child2Path, DeviceType.PartitionDevice, null, driveDP, 0);

        var ramdisk = BcdStore.SystemStore.CreateObject(Guid.NewGuid(), BcdObjectType.Device);
        var svDP = GetDevicePath(SVDrive);
        ramdisk.SetPartitionDeviceElement(BcdElementType.BcdDeviceSdiDevice, DeviceType.PartitionDevice, null, svDP);
        ramdisk.SetStringElement(BcdElementType.BcdDeviceSdiPath, Path.Combine(SVPath[2..], "Boot", "boot.sdi"));

        var pe = BcdStore.SystemStore.CreateObject(Guid.NewGuid(), BcdObjectType.BootLoader);
        var pePath = Path.Combine(SVPath[2..], "Boot", $"{arch}.wim");
        pe.SetStringElement(BcdElementType.BcdLibraryDescription, "SimpleVhd PE");
        pe.SetFileDeviceElement(BcdElementType.BcdLibraryApplicationDevice, DeviceType.RamdiskDevice, ramdisk, pePath, DeviceType.PartitionDevice, null, svDP);
        pe.SetStringElement(BcdElementType.BcdLibraryApplicationPath, $@"\windows\system32\winload.{(IsWindowsUefi() ? "efi" : "exe")}");
        pe.SetObjectListElement(BcdElementType.BcdLibraryInheritedObjects, BcdStore.SystemStore.OpenObject(WellKnownGuids.BootLoaderSettings));
        pe.SetFileDeviceElement(BcdElementType.BcdOSLoaderOSDevice, DeviceType.RamdiskDevice, ramdisk, pePath, DeviceType.PartitionDevice, null, svDP);
        pe.SetStringElement(BcdElementType.BcdOSLoaderSystemRoot, "\\windows");
        pe.SetBooleanElement(BcdElementType.BcdOSLoaderDetectKernelAndHal, true);
        pe.SetBooleanElement(BcdElementType.BcdOSLoaderWinPEMode, true);

        var bootmgr = BcdStore.SystemStore.OpenObject(WellKnownGuids.BootMgr);
        bootmgr.SetIntegerElement(BcdElementType.BcdBootMgrTimeout, 5);
        bootmgr.SetObjectListElement(BcdElementType.BcdBootMgrDisplayOrder, [.. ((BcdObject[])bootmgr.GetElement(BcdElementType.BcdBootMgrDisplayOrder)), pe]);

        using FileStream fs = new(Path.Combine(SVPath, SettingsFileName), FileMode.Create, FileAccess.Write, FileShare.None);
        using Utf8JsonWriter writer = new(fs, new JsonWriterOptions() { Indented = true });

        new JsonObject() {
            { "$schema", SettingsSchemaUrl },
            {
                nameof(Settings.Instances),
                (JsonArray)([
            new JsonObject() {
                { nameof(Vhd.Name), Name },
                { nameof(Vhd.Directory), VhdPath },
                { nameof(Vhd.FileName), VhdFileName },
                { nameof(Vhd.Style), Style.Normal.ToString() },
                { nameof(Vhd.Type), VhdType.ToString() },
                { nameof(Vhd.Format), VhdFormat.ToString() },
                { nameof(Vhd.ParentGuid), parent.Id.ToString("B") },
                { nameof(Vhd.Child1Guid), child1.Id.ToString("B") },
                { nameof(Vhd.Child2Guid), child2.Id.ToString("B") }
            },
                ])
            },
            { nameof(Settings.RamdiskGuid), ramdisk.Id.ToString("B") },
            { nameof(Settings.PEGuid), pe.Id.ToString("B") },
        }.WriteTo(writer);
    }
}
