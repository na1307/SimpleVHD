namespace SimpleVhd.Bcd;

public sealed record class BcdDeviceLocateStringData : BcdDeviceLocateData {
    internal BcdDeviceLocateStringData(DeviceType dType, BcdObject? addOptions, uint type, string path) : base(dType, addOptions, type) => Path = path;

    public string Path { get; }
}
