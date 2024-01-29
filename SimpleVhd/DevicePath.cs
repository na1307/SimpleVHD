using System.Runtime.InteropServices;
using System.Text;

namespace SimpleVhd;

public static class DevicePath {
    public static string GetDevicePath(string drive) {
        const uint length = 128;
        StringBuilder sb = new((int)length);

        return QueryDosDeviceW(drive, sb, length) != 0 ? sb.ToString() : throw new SimpleVhdException("QueryDosDevice Failed: " + Marshal.GetLastWin32Error().ToString());
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    private static extern uint QueryDosDeviceW([MarshalAs(UnmanagedType.LPWStr)] string lpDeviceName, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpTargetPath, uint ucchMax);
}
