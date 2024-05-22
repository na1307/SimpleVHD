using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleVhd;

public static class StaticFunctions {
    public static string GetDevicePath(string drive) {
        const uint length = 128;
        StringBuilder sb = new((int)length);

        return QueryDosDeviceW(drive, sb, length + 1) != 0 ? sb.ToString() : throw new DevicePathMapperException("QueryDosDevice Failed: " + Marshal.GetLastPInvokeErrorMessage());
    }

    public static string FromDevicePath(string devicePath) {
        DriveInfo? drive = Array.Find(DriveInfo.GetDrives(), d => devicePath.StartsWith(GetDevicePath(d.GetDriveLetterAndColon()), StringComparison.InvariantCultureIgnoreCase));

        return drive != null ? devicePath.ReplaceFirst(GetDevicePath(drive.GetDriveLetterAndColon()), drive.GetDriveLetterAndColon()) : string.Empty;
    }

    public static string GetSystemVhdPath() {
        var queryObj = new ManagementObjectSearcher(@"root\Microsoft\Windows\Storage", $"SELECT * FROM MSFT_PhysicalDisk WHERE DeviceID='{getSystemDiskNumber()}'").Get().Cast<ManagementBaseObject>().First();
        var pl = queryObj["PhysicalLocation"].ToString();

        return pl![..22] is @"\Device\HarddiskVolume" ? FromDevicePath(pl) : string.Empty;

        static int getSystemDiskNumber() => (int)(uint)new ManagementObjectSearcher("ASSOCIATORS OF {Win32_LogicalDisk.DeviceID='C:'} WHERE AssocClass=Win32_LogicalDiskToPartition").Get().Cast<ManagementBaseObject>().First()["DiskIndex"];
    }

    public static bool IsWindowsUefi() {
        _ = GetFirmwareType(out var firmwareType);

        return firmwareType == 2;
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

    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetFirmwareType(out int firmwareType);

    private sealed class DevicePathMapperException(string message) : SimpleVhdException(message);
}
