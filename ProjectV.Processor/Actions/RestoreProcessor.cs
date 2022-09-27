#nullable enable
namespace ProjectV.Processor.Actions;

internal class RestoreProcessor : ActionProcessor {
    protected sealed override bool NeedBackup => true;
    protected sealed override bool AfterRebuild => true;
    protected sealed override bool AfterRevert => true;
    protected virtual VhdType VType => PVConfig.Instance.VhdType;

    public RestoreProcessor() : base("복원", PVConfig.Instance[ShutdownType.Restore]) { }
    protected RestoreProcessor(string operation) : base(operation) { }

    protected override void DoProcessCore() {
        File.Delete(VHDDir + PVConfig.Instance.VhdFile);
        ProcessDiskpart($"create vdisk file \"{VHDDir}{PVConfig.Instance.VhdFile}\" source \"{BackupDir}{PVConfig.Instance.VhdFile}\" type {VType}");
    }
}