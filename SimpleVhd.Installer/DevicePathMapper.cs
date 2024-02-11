using Bluehill;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleVhd.Installer;

public static class DevicePathMapper {
    public static string GetDevicePath(string drive) {
        const uint length = 128;
        StringBuilder sb = new((int)length);

        return QueryDosDeviceW(drive, sb, length + 1) != 0 ? sb.ToString() : throw new SimpleVhdException("QueryDosDevice Failed: " + Marshal.GetLastPInvokeErrorMessage());
    }

    public static string FromDevicePath(string devicePath) {
        DriveInfo? drive = Array.Find(DriveInfo.GetDrives(), d => devicePath.StartsWith(GetDevicePath(d.GetDriveLetter()), StringComparison.InvariantCultureIgnoreCase));

        return drive != null ? devicePath.ReplaceFirst(GetDevicePath(drive.GetDriveLetter()), drive.GetDriveLetter()) : string.Empty;
    }

    private static string ReplaceFirst(this string text, string search, string replace) {
        var pos = text.IndexOf(search);

        if (pos < 0) {
            return text;
        }

        return text[..pos] + replace + text[(pos + search.Length)..];
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    private static extern uint QueryDosDeviceW([MarshalAs(UnmanagedType.LPWStr)] string lpDeviceName, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpTargetPath, uint ucchMax);
}
