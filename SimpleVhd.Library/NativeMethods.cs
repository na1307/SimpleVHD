using System.Runtime.InteropServices;
using System.Text;

namespace SimpleVhd;

internal static class NativeMethods {
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    public static extern uint QueryDosDeviceW(
        [MarshalAs(UnmanagedType.LPWStr)] string lpDeviceName,
        [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpTargetPath,
        uint ucchMax);

    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetFirmwareType(out int firmwareType);
}
