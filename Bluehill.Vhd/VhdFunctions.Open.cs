namespace Bluehill.Vhd;

public static partial class VhdFunctions {
    public static SafeVirtualDiskHandle GetVhdHandle(string path) {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        return getVhdHandleCore(path);
    }

    private static SafeVirtualDiskHandle getVhdHandleCore(string path) {
        var vst = getVst(path);
        var result = NativeMethods.OpenVirtualDisk(
            in vst,
            path,
            VirtualDiskAccessMask.All,
            OpenVirtualDiskOptions.None,
            nint.Zero,
            out var handle);

        return result == 0 ? handle : throw new OperationFailedException(Marshal.GetPInvokeErrorMessage((int)result));
    }
}
