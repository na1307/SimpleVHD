using System.Runtime.InteropServices;

namespace SimpleVHD;

public static class Firmware {
    [DllImport("kernel32.dll", EntryPoint = "GetFirmwareEnvironmentVariableW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    private static extern int GetFirmwareType(string lpName, string lpGUID, IntPtr pBuffer, uint size);

    public static bool IsWindowsUEFI { get; }

    static Firmware() {
        GetFirmwareType("", "{00000000-0000-0000-0000-000000000000}", IntPtr.Zero, 0);
        IsWindowsUEFI = Marshal.GetLastWin32Error() != 1;
    }
}