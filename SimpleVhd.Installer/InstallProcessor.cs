using Bluehill;
using System.Management;

namespace SimpleVhd.Installer;

public abstract class InstallProcessor {
    public const string NoName = "(이름 없음)";

    public string SVDrive { get; set; } = string.Empty;
    public string SVPath { get; set; } = string.Empty;
    public string SVDir { get; set; } = string.Empty;
    public string VhdDrive { get; set; } = string.Empty;
    public string VhdPath { get; set; } = string.Empty;
    public string VhdFileName { get; set; } = string.Empty;
    public string Name { get; set; } = NoName;
    public VhdType Type { get; set; }
    public VhdFormat Format { get; set; }

    public void CheckRequirements() {
        if (!Environment.Is64BitOperatingSystem) {
            throw new RequirementsNotMetException("64비트 운영 체제만 지원합니다.");
        }

        SVDrive = DriveInfo.GetDrives().First(d => d.DriveType == DriveType.Fixed && Directory.Exists(Path.Combine(d.Name, DirName))).GetDriveLetter();
        SVPath = Path.DirectorySeparatorChar.ToString() + DirName + Path.DirectorySeparatorChar.ToString();
        SVDir = SVDrive + SVPath;
        _ = getVhdPath(getSystemDiskNumber());
    }

    public abstract void InstallProcess();

    protected static int getSystemDiskNumber() => (int)(uint)new ManagementObjectSearcher("ASSOCIATORS OF {Win32_LogicalDisk.DeviceID='C:'} WHERE AssocClass=Win32_LogicalDiskToPartition").Get().Cast<ManagementBaseObject>().First()["DiskIndex"];

    protected static string getVhdPath(int number) {
        var queryObj = new ManagementObjectSearcher(@"root\Microsoft\Windows\Storage", "SELECT * FROM MSFT_PhysicalDisk WHERE DeviceID=\"" + number.ToString() + "\"").Get().Cast<ManagementBaseObject>().First();
        var pl = queryObj["PhysicalLocation"].ToString();

        if (pl![..22] is not @"\Device\HarddiskVolume") {
            throw new RequirementsNotMetException("현재 가상 디스크로 부팅하지 않았습니다.");
        }

        return DevicePathMapper.FromDevicePath(pl);
    }
}
