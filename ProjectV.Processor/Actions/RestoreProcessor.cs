#nullable enable
namespace ProjectV.Processor.Actions;

internal class RestoreProcessor : ActionProcessor {
    protected sealed override bool NeedBackup => true;
    protected sealed override bool AfterRebuild => true;
    protected sealed override bool AfterRevert => true;
    protected virtual VhdType VType => Config.VhdType;

    public RestoreProcessor() : base("복원", Config[ShutdownType.Restore]) { }
    protected RestoreProcessor(string operation) : base(operation) { }

    protected override void DoProcessCore() {
        File.Delete(VHDDir + VF);
        ProcessDiskpart($"create vdisk file \"{VHDDir}{VF}\" source \"{BackupDir}{VF}\" type {VType}");
    }
}