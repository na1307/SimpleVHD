namespace Bluehill.Vhd;

public sealed class SafeVirtualDiskHandle : SafeHandleZeroOrMinusOneIsInvalid {
    public SafeVirtualDiskHandle() : base(true) { }
    public SafeVirtualDiskHandle(nint preexistingHandle, bool ownsHandle) : base(ownsHandle) => SetHandle(preexistingHandle);

    protected override bool ReleaseHandle() => NativeMethods.CloseHandle(handle);
}
