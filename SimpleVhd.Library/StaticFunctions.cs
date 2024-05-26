using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using static SimpleVhd.NativeMethods;

namespace SimpleVhd;

public static class StaticFunctions {
    public static string GetDevicePath(string drive) {
        const uint length = 128;
        StringBuilder sb = new((int)length);

        return QueryDosDeviceW(drive, sb, length + 1) != 0 ? sb.ToString() : throw new DevicePathMapperException("QueryDosDevice Failed: " + Marshal.GetLastPInvokeErrorMessage());
    }

    public static string FromDevicePath(string devicePath) {
        var drive = Array.Find(DriveInfo.GetDrives(), d => devicePath.StartsWith(GetDevicePath(d.GetDriveLetterAndColon()), StringComparison.InvariantCultureIgnoreCase));

        return drive != null ? devicePath.ReplaceFirst(GetDevicePath(drive.GetDriveLetterAndColon()), drive.GetDriveLetterAndColon()) : string.Empty;
    }

    public static string GetSystemVhdPath() {
        using var searcher1 = new ManagementObjectSearcher(@"root\Microsoft\Windows\Storage", $"SELECT * FROM MSFT_PhysicalDisk WHERE DeviceID='{getSystemDiskNumber()}'");
        using var queryObj = searcher1.Get().Cast<ManagementBaseObject>().First();
        var pl = queryObj["PhysicalLocation"].ToString();

        return pl![..22] is @"\Device\HarddiskVolume" ? FromDevicePath(pl) : string.Empty;

        static int getSystemDiskNumber() {
            using var searcher2 = new ManagementObjectSearcher("ASSOCIATORS OF {Win32_LogicalDisk.DeviceID='C:'} WHERE AssocClass=Win32_LogicalDiskToPartition");

            return (int)(uint)searcher2.Get().Cast<ManagementBaseObject>().First()["DiskIndex"];
        }
    }

    public static bool IsWindowsUefi() {
        _ = GetFirmwareType(out var firmwareType);

        return firmwareType == 2;
    }

    private static string ReplaceFirst(this string text, string search, string replace) {
        var pos = text.IndexOf(search);

        return pos < 0 ? text : text[..pos] + replace + text[(pos + search.Length)..];
    }

    private sealed class DevicePathMapperException(string message) : SimpleVhdException(message);
}
