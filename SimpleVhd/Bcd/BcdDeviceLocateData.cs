namespace SimpleVhd.Bcd;

public abstract record class BcdDeviceLocateData : BcdDeviceData {
    private protected BcdDeviceLocateData(DeviceType dType, BcdObject? addOptions, uint type) : base(dType, addOptions) => Type = type;

    public uint Type { get; }
}
