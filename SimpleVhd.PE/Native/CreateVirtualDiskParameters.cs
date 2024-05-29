using System.Runtime.InteropServices;

namespace SimpleVhd.PE.Native;

[StructLayout(LayoutKind.Sequential)]
public struct CreateVirtualDiskParameters {
    private readonly int Version = 1;
    public Guid UniqueId;
    public ulong MaximumSize;
    public uint BlockSizeInBytes;
    public uint SectorSizeInBytes;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string? ParentPath;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string? SourcePath;

    public CreateVirtualDiskParameters() { }
}
