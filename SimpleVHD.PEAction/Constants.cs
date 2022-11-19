namespace SimpleVHD.PEAction;

internal static class Constants {
    public const int RestartCode = 0;
    public const int ShutdownCode = 1307;
    public static readonly string PVDir = string.Empty;
    public static readonly string BackupDir = string.Empty;
    public static readonly string VhdDir = string.Empty;

    static Constants() {
        var drvs = DriveInfo.GetDrives().Where(Extensions.CheckFixed);

        var pvDirs = from d in drvs
                     where File.Exists(d.Name + DirName + Path.DirectorySeparatorChar.ToString() + ConfigName)
                     select d.Name;

        var backupDirs = from d in drvs
                         where Directory.Exists(d.Name + BackupDirName)
                         select d.Name;

        var vhdDirs = from d in drvs
                      where File.Exists(d.GetLetter() + PVConfig.Instance.VhdDirectory + PVConfig.Instance.VhdFile)
                      select d.GetLetter();

        if (pvDirs.Any()) PVDir = pvDirs.First() + DirName + Path.DirectorySeparatorChar.ToString();

        if (backupDirs.Any()) {
            BackupDir = backupDirs.First() + BackupDirName + Path.DirectorySeparatorChar.ToString();
        } else if (Directory.Exists(PVDir + IncludedBackupDirName)) {
            BackupDir = PVDir + IncludedBackupDirName + Path.DirectorySeparatorChar.ToString();
        }

        if (vhdDirs.Any()) VhdDir = vhdDirs.First() + PVConfig.Instance.VhdDirectory;
    }
}