using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Bluehill.Vhd;

public static partial class VhdFunctions {
    public static SafeFileHandle GetVhdHandle(string path) {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        return getVhdHandleCore(path);
    }

    private static SafeFileHandle getVhdHandleCore(string path) {
        var vst = getVst(path);
        var result = NativeMethods.OpenVirtualDisk(
            in vst,
            path,
            VirtualDiskAccessMask.None,
            OpenVirtualDiskOptions.None,
            nint.Zero,
            out var handle);

        return result == 0 ? new(handle, true) : throw new OperationFailedException(Marshal.GetPInvokeErrorMessage((int)result));
    }
}
