using Bluehill.Bcd;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SimpleVhd.Installer;

public sealed class NewInstallProcessor : InstallProcessor {
    public override void InstallProcess() {
        Directory.CreateDirectory(Path.Combine(SVDrive, SVDirName, BackupDirName));

        var arch = RuntimeInformation.ProcessArchitecture switch {
            Architecture.X64 => "x64",
            Architecture.Arm64 => "arm64",
            _ => throw new NotImplementedException(),
        };

        var parrent = BcdStore.SystemStore.OpenObject(WellKnownGuids.Current);

        var child1 = BcdStore.SystemStore.CopyObject(parrent, CopyObjectOptions.CreateNewId);
        child1.SetVhdDeviceElement(BcdElementType.BcdLibraryApplicationDevice, Path.Combine(VhdPath, $"{Child1FileName}.{VhdFormat.ToString().ToLower()}"), DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);
        child1.SetVhdDeviceElement(BcdElementType.BcdOSLoaderOSDevice, Path.Combine(VhdPath, $"{Child1FileName}.{VhdFormat.ToString().ToLower()}"), DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);

        var child2 = BcdStore.SystemStore.CopyObject(parrent, CopyObjectOptions.CreateNewId);
        child2.SetVhdDeviceElement(BcdElementType.BcdLibraryApplicationDevice, Path.Combine(VhdPath, $"{Child2FileName}.{VhdFormat.ToString().ToLower()}"), DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);
        child2.SetVhdDeviceElement(BcdElementType.BcdOSLoaderOSDevice, Path.Combine(VhdPath, $"{Child2FileName}.{VhdFormat.ToString().ToLower()}"), DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);

        var ramdisk = BcdStore.SystemStore.CreateObject(Guid.NewGuid(), BcdObjectType.Device);
        ramdisk.SetPartitionDeviceElement(BcdElementType.BcdDeviceSdiDevice, DeviceType.PartitionDevice, null, GetDevicePath(SVDrive));
        ramdisk.SetStringElement(BcdElementType.BcdDeviceSdiPath, Path.Combine(SVPath, "Boot", "boot.sdi"));

        var pe = BcdStore.SystemStore.CreateObject(Guid.NewGuid(), BcdObjectType.BootLoader);
        pe.SetStringElement(BcdElementType.BcdLibraryDescription, "SimpleVHD PE");
        pe.SetFileDeviceElement(BcdElementType.BcdLibraryApplicationDevice, DeviceType.RamdiskDevice, ramdisk, Path.Combine(SVPath, "Boot", $"{arch}.wim"), DeviceType.PartitionDevice, null, GetDevicePath(SVDrive));
        pe.SetStringElement(BcdElementType.BcdLibraryApplicationPath, $@"\windows\system32\winload.{(IsWindowsUefi() ? "efi" : "exe")}");
        pe.SetObjectListElement(BcdElementType.BcdLibraryInheritedObjects, BcdStore.SystemStore.OpenObject(WellKnownGuids.BootLoaderSettings));
        pe.SetFileDeviceElement(BcdElementType.BcdOSLoaderOSDevice, DeviceType.RamdiskDevice, ramdisk, Path.Combine(SVPath, "Boot", $"{arch}.wim"), DeviceType.PartitionDevice, null, GetDevicePath(SVDrive));
        pe.SetStringElement(BcdElementType.BcdOSLoaderSystemRoot, "\\windows");
        pe.SetBooleanElement(BcdElementType.BcdOSLoaderDetectKernelAndHal, true);
        pe.SetBooleanElement(BcdElementType.BcdOSLoaderWinPEMode, true);

        var bootmgr = BcdStore.SystemStore.OpenObject(WellKnownGuids.BootMgr);
        bootmgr.SetIntegerElement(BcdElementType.BcdBootMgrTimeout, 5);
        bootmgr.SetObjectListElement(BcdElementType.BcdBootMgrDisplayOrder, [.. ((BcdObject[])bootmgr.GetElement(BcdElementType.BcdBootMgrDisplayOrder)), pe]);

        using FileStream fs = new(Path.Combine(SVDrive, SVDirName, SettingsFileName), FileMode.Create, FileAccess.Write, FileShare.None);
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
                { nameof(Vhd.ParentGuid), parrent.Id.ToString("B") },
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
