#nullable enable
global using static ProjectV.Processor.Constants;

namespace ProjectV.Processor;

internal static class Constants {
    public const int RestartCode = 0;
    public const int ShutdownCode = 1307;
    public static readonly string? PVDrv, PVDir, BackupDrv, BackupDir, VHDDrv, VHDDir;

    static Constants() {
        var config = PVConfig.Instance;

        foreach (var drv in Directory.GetLogicalDrives()) {
            if (File.Exists(drv + DirName + "\\" + ConfigName)) {
                PVDrv = drv.Substring(0, 2);
                PVDir = drv + DirName + "\\";
            }

            if (Directory.Exists(drv + BackupDirName)) {
                BackupDrv = drv.Substring(0, 2);
                BackupDir = drv + BackupDirName + "\\";
            }

            if (File.Exists(drv.Substring(0, 2) + config.VhdDirectory + config.VhdFile)) {
                VHDDrv = drv.Substring(0, 2);
                VHDDir = drv.Substring(0, 2) + config.VhdDirectory;
            }
        }
    }
}