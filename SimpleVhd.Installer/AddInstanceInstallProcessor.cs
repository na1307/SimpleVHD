using Bluehill.Bcd;

namespace SimpleVhd.Installer;

public sealed class AddInstanceInstallProcessor : InstallProcessor {
    public override void InstallProcess() {
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

        var settings = Settings.Instance;

        settings.Instances.Add(new() {
            Name = Name,
            Directory = VhdPath,
            FileName = VhdFileName,
            Style = Style.Normal,
            Type = VhdType,
            Format = VhdFormat,
            ParentGuid = parent.Id,
            Child1Guid = child1.Id,
            Child2Guid = child2.Id,
        });

        settings.SaveSettings();
    }
}
