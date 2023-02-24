namespace SimpleVHD.PEAction.Actions;

internal class Restore : Action {
    protected override string Name => "복원";
    protected VhdType VType { get; init; } = PVConfig.Instance.VhdType;

    public Restore() {
        NeedBackup = true;
        AfterRebuild = true;
        AfterRevert = true;
        Shutdown = PVConfig.Instance.GetShutdown(DoAction.DoRestore);
    }

    protected override void RunCore() {
        File.Delete(VhdDir + PVConfig.Instance.VhdFile);
        ProcessDiskpart($"create vdisk file \"{VhdDir}{PVConfig.Instance.VhdFile}\" source \"{BackupDir}{PVConfig.Instance.VhdFile}\" type {VType}");
    }
}