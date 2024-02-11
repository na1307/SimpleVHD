using System.Text;

namespace SimpleVhd.PE;

public static class PEConstants {
    public static readonly string SVDir = DriveInfo.GetDrives().First(d => d.DriveType == DriveType.Fixed && File.Exists(d.Name + DirName + Path.DirectorySeparatorChar.ToString() + SettingsFileName)).Name + DirName;
    public static readonly string BackupDir = Path.Combine(SVDir, BackupDirName);
    public static readonly Encoding SystemEncoding = CodePagesEncodingProvider.Instance.GetEncoding(0)!;
}
