using System.Management;
using System.Text.RegularExpressions;

namespace SimpleVhd.Installer;

public abstract class InstallProcessor {
    public string SVDrive { get; set; } = string.Empty;
    public string SVPath { get; set; } = string.Empty;
    public string SVDir { get; set; } = string.Empty;
    public string VhdDrive { get; set; } = string.Empty;
    public string VhdPath { get; set; } = string.Empty;
    public string VhdName { get; set; } = string.Empty;
    public VhdType Type { get; set; }
    public VhdFormat Format { get; set; }

    public void CheckRequirements() {
        if (!Environment.Is64BitOperatingSystem) {
            throw new RequirementsNotMetException("64비트 운영 체제만 지원합니다.");
        }

        IEnumerable<string> drvs = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed && Directory.Exists(Path.Combine(d.Name, DirName))).Select(d => d.Name.TrimEnd('\\'));

        if (drvs.Any()) {
            SVDrive = drvs.First();
            SVPath = Path.DirectorySeparatorChar.ToString() + DirName + Path.DirectorySeparatorChar.ToString();
            SVDir = SVDrive + SVPath;
        } else {
            throw new RequirementsNotMetException("아무 드라이브의 루트에 " + DirName + " 폴더가 없습니다.");
        }

        IEnumerable<string> requires = ((string[])["Boot\\x64.wim", "Boot\\arm64.wim", "Boot\\boot.sdi"]).Where(f => !File.Exists(Path.Combine(SVDir, f)));

        if (requires.Any()) {
            throw new RequirementsNotMetException(Path.GetFileName(requires.First()) + " 파일이 없습니다.");
        }

        _ = getVhdPath(getSystemDiskNumber());
    }

    public abstract void InstallProcess();

    protected static int getSystemDiskNumber() {
        const string q = "SELECT * FROM Win32_LogicalDiskToPartition";
        var a = new ManagementObjectSearcher(q).Get().Cast<ManagementObject>().Where(drive => ((string)drive["Dependent"]).Contains("C:")).Select(drive => (string)drive["Antecedent"]).First();

        return int.Parse(Regex.Match(a, "Disk #(?<number>[0-9]+), Partition #[0-9]+").Groups["number"].Value);
    }

    protected static string getVhdPath(int number) {
        ManagementBaseObject queryObj = new ManagementObjectSearcher(@"root\Microsoft\Windows\Storage", "SELECT * FROM MSFT_PhysicalDisk WHERE DeviceID=\"" + number.ToString() + "\"").Get().Cast<ManagementBaseObject>().First();
        var pl = queryObj["PhysicalLocation"].ToString();

        if (pl![..22] is not @"\Device\HarddiskVolume") {
            throw new RequirementsNotMetException("현재 가상 디스크로 부팅하지 않았습니다.");
        }

        return DevicePathMapper.FromDevicePath(pl);
    }
}
