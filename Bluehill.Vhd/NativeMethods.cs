namespace Bluehill.Vhd;

internal static class NativeMethods {
    [DllImport("virtdisk.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    public static extern uint CreateVirtualDisk(
        in VirtualStorageType virtualStorageType,
        [MarshalAs(UnmanagedType.LPWStr)] string path,
        VirtualDiskAccessMask virtualDiskAccessMask,
        nint securityDescriptor,
        CreateVirtualDiskOptions flags,
        uint providerSpecificFlags,
        in CreateVirtualDiskParameters parameters,
        nint overlapped,
        out SafeFileHandle handle);

    [DllImport("virtdisk.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    public static extern uint OpenVirtualDisk(
        in VirtualStorageType virtualStorageType,
        [MarshalAs(UnmanagedType.LPWStr)] string path,
        VirtualDiskAccessMask virtualDiskAccessMask,
        OpenVirtualDiskOptions flags,
        nint parameters,
        out SafeFileHandle handle);

    [DllImport("virtdisk.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    public static extern uint AttachVirtualDisk(
        SafeFileHandle virtualDiskHandle,
        nint securityDescriptor,
        AttachVirtualDiskOptions flag,
        uint providerSpecificFlags,
        nint parameters,
        nint overlapped);

    [DllImport("virtdisk.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    public static extern uint DetachVirtualDisk(
        SafeFileHandle virtualDiskHandle,
        DetachVirtualDiskOptions flags,
        uint providerSpecificFlags);
}
