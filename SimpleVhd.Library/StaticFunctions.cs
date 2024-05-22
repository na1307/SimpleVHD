using System.Management;

namespace SimpleVhd;

public static class StaticFunctions {
    public static string GetSystemVhdPath() {
        var queryObj = new ManagementObjectSearcher(@"root\Microsoft\Windows\Storage", $"SELECT * FROM MSFT_PhysicalDisk WHERE DeviceID='{getSystemDiskNumber()}'").Get().Cast<ManagementBaseObject>().First();
        var pl = queryObj["PhysicalLocation"].ToString();

        return pl![..22] is @"\Device\HarddiskVolume" ? DevicePathMapper.FromDevicePath(pl) : string.Empty;

        static int getSystemDiskNumber() => (int)(uint)new ManagementObjectSearcher("ASSOCIATORS OF {Win32_LogicalDisk.DeviceID='C:'} WHERE AssocClass=Win32_LogicalDiskToPartition").Get().Cast<ManagementBaseObject>().First()["DiskIndex"];
    }
}
