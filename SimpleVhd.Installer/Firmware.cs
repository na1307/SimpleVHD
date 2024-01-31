using System.Runtime.InteropServices;

namespace SimpleVhd.Installer;

public static class Firmware {
    public static bool IsWindowsUEFI { get; }

    static Firmware() {
        _ = GetFirmwareType(out int i);
        IsWindowsUEFI = i == 2;

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetFirmwareType(out int firmwareType);
    }
}
