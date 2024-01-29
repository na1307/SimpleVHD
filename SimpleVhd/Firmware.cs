using System.Runtime.InteropServices;

namespace SimpleVhd;

public static class Firmware {
    public static bool IsWindowsUEFI { get; }

    static Firmware() {
        _ = GetFirmwareType("", "{00000000-0000-0000-0000-000000000000}", IntPtr.Zero, 0);
        IsWindowsUEFI = Marshal.GetLastWin32Error() != 1;

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "GetFirmwareEnvironmentVariableW", ExactSpelling = true, SetLastError = true)]
        static extern uint GetFirmwareType([MarshalAs(UnmanagedType.LPWStr)] string lpName, [MarshalAs(UnmanagedType.LPWStr)] string lpGUID, IntPtr pBuffer, uint size);
    }
}
