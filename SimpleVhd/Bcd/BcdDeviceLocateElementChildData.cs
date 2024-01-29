namespace SimpleVhd.Bcd;

public sealed record class BcdDeviceLocateElementChildData : BcdDeviceLocateData {
    internal BcdDeviceLocateElementChildData(DeviceType dType, BcdObject? addOptions, uint type, uint element, BcdDeviceData parent) : base(dType, addOptions, type) {
        Element = element;
        Parent = parent;
    }

    public uint Element { get; }
    public BcdDeviceData Parent { get; }
}
