namespace SimpleVhd.Bcd;

public enum DeviceType {
    BootDevice = 1,
    PartitionDevice,
    FileDevice,
    RamdiskDevice,
    UnknownDevice,
    QualifiedPartition,
    LocateDevice,
    LocateExDevice
}
