namespace Bluehill.Vhd;

public static partial class VhdFunctions {
    public static void AttachVhd(string path, bool readOnly = false, bool noDriveLetter = false) {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        using var handle = GetVhdHandle(path);

        attachVhdCore(handle, readOnly, noDriveLetter);
    }

    public static void AttachVhd(SafeVirtualDiskHandle vhdHandle, bool readOnly = false, bool noDriveLetter = false)
        => attachVhdCore(vhdHandle, readOnly, noDriveLetter);

    private static void attachVhdCore(SafeVirtualDiskHandle handle, bool readOnly, bool noDriveLetter) {
        var options = AttachVirtualDiskOptions.PermanentLifetime;

        if (readOnly) {
            options |= AttachVirtualDiskOptions.ReadOnly;
        }

        if (noDriveLetter) {
            options |= AttachVirtualDiskOptions.NoDriveLetter;
        }

        var result = NativeMethods.AttachVirtualDisk(
            handle,
            nint.Zero,
            options,
            0,
            nint.Zero,
            nint.Zero);

        if (result != 0) {
            throw new VhdOperationFailedException(Marshal.GetPInvokeErrorMessage((int)result));
        }
    }
}
