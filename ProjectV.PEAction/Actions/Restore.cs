namespace ProjectV.PEAction.Actions;

internal class Restore : Action {
    protected override string Name => "복원";
    protected sealed override bool NeedBackup => true;
    protected sealed override bool AfterRebuild => true;
    protected sealed override bool AfterRevert => true;
    protected override bool Shutdown => PVConfig.Instance[DoAction.DoRestore];
    protected virtual VhdType VType => PVConfig.Instance.VhdType;

    protected override void RunCore() {
        File.Delete(VhdDir + PVConfig.Instance.VhdFile);
        ProcessDiskpart($"create vdisk file \"{VhdDir}{PVConfig.Instance.VhdFile}\" source \"{BackupDir}{PVConfig.Instance.VhdFile}\" type {VType}");
    }
}