namespace SimpleVHD.PEAction.Actions;

internal class Backup : Action {
    public Backup() {
        Name = "백업";
        Shutdown = PVConfig.Instance.GetShutdown(DoAction.DoBackup);
    }

    protected override void RunCore() {
        File.Delete(BackupDir + PVConfig.Instance.VhdFile);
        ProcessDiskpart($"create vdisk file \"{BackupDir}{PVConfig.Instance.VhdFile}\" source \"{VhdDir}{PVConfig.Instance.VhdFile}\" type expandable");
    }
}