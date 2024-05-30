namespace Bluehill.Vhd;

public static partial class VhdFunctions {
    public static void DetachVhd(string path) {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        using var handle = GetVhdHandle(path);

        detachVhdCore(handle);
    }

    public static void DetachVhd(SafeFileHandle vhdHandle) => detachVhdCore(vhdHandle);

    private static void detachVhdCore(SafeFileHandle handle) {
        var result = NativeMethods.DetachVirtualDisk(handle, DetachVirtualDiskOptions.None, 0);

        if (result != 0) {
            throw new OperationFailedException(Marshal.GetPInvokeErrorMessage((int)result));
        }
    }
}
