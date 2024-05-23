using Bluehill.Bcd;

namespace SimpleVhd.Installer;

public class AddInstanceInstallProcessor : InstallProcessor {
    public override void InstallProcess() {
        var parent = BcdStore.SystemStore.OpenObject(WellKnownGuids.Current);

        var child1 = BcdStore.SystemStore.CopyObject(parent, CopyObjectOptions.CreateNewId);
        child1.SetVhdDeviceElement(BcdElementType.BcdLibraryApplicationDevice, Path.Combine(VhdPath, $"{Child1FileName}.{VhdFormat.ToString().ToLower()}"), DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);
        child1.SetVhdDeviceElement(BcdElementType.BcdOSLoaderOSDevice, Path.Combine(VhdPath, $"{Child1FileName}.{VhdFormat.ToString().ToLower()}"), DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);

        var child2 = BcdStore.SystemStore.CopyObject(parent, CopyObjectOptions.CreateNewId);
        child2.SetVhdDeviceElement(BcdElementType.BcdLibraryApplicationDevice, Path.Combine(VhdPath, $"{Child2FileName}.{VhdFormat.ToString().ToLower()}"), DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);
        child2.SetVhdDeviceElement(BcdElementType.BcdOSLoaderOSDevice, Path.Combine(VhdPath, $"{Child2FileName}.{VhdFormat.ToString().ToLower()}"), DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);

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
