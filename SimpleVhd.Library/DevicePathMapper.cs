using System.Runtime.InteropServices;
using System.Text;

namespace SimpleVhd;

public static class DevicePathMapper {
    public static string GetDevicePath(string drive) {
        const uint length = 128;
        StringBuilder sb = new((int)length);

        return QueryDosDeviceW(drive, sb, length + 1) != 0 ? sb.ToString() : throw new DevicePathMapperException("QueryDosDevice Failed: " + Marshal.GetLastPInvokeErrorMessage());
    }

    public static string FromDevicePath(string devicePath) {
        DriveInfo? drive = Array.Find(DriveInfo.GetDrives(), d => devicePath.StartsWith(GetDevicePath(d.GetDriveLetterAndColon()), StringComparison.InvariantCultureIgnoreCase));

        return drive != null ? devicePath.ReplaceFirst(GetDevicePath(drive.GetDriveLetterAndColon()), drive.GetDriveLetterAndColon()) : string.Empty;
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

    private sealed class DevicePathMapperException(string message) : SimpleVhdException(message);
}
