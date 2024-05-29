using System.Runtime.InteropServices;

namespace SimpleVhd.PE.Native;

[StructLayout(LayoutKind.Sequential)]
public struct VirtualStorageType {
    public static readonly Guid Microsoft = Guid.Parse("{EC984AEC-A0F9-47e9-901F-71415A66345B}");

    public VirtualStorageTypeDevice DeviceId;
    public Guid VendorId;
}
