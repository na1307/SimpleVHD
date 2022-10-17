namespace ProjectV.PEAction.Actions;

internal class Backup : Action {
    protected override string Name => "백업";
    protected override bool Shutdown => PVConfig.Instance[DoAction.DoBackup];

    protected override void RunCore() {
        File.Delete(BackupDir + PVConfig.Instance.VhdFile);
        ProcessDiskpart($"create vdisk file \"{BackupDir}{PVConfig.Instance.VhdFile}\" source \"{VhdDir}{PVConfig.Instance.VhdFile}\" type expandable");
    }
}