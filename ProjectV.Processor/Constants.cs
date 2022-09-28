#nullable enable
global using static ProjectV.Processor.Constants;

namespace ProjectV.Processor;

internal static class Constants {
    public const int RestartCode = 0;
    public const int ShutdownCode = 1307;
    public static readonly string PVDir = string.Empty;
    public static readonly string BackupDir = string.Empty;
    public static readonly string VhdDir = string.Empty;

    static Constants() {
        foreach (var drv in Directory.GetLogicalDrives()) {
            if (File.Exists(drv + DirName + "\\" + ConfigName)) PVDir = drv + DirName + "\\";
            if (Directory.Exists(drv + BackupDirName)) BackupDir = drv + BackupDirName + "\\";
            if (File.Exists(drv.Substring(0, 2) + PVConfig.Instance.VhdDirectory + PVConfig.Instance.VhdFile)) VhdDir = drv.Substring(0, 2) + PVConfig.Instance.VhdDirectory;
        }
    }
}