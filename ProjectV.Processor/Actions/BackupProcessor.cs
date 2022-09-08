#nullable enable
namespace ProjectV.Processor.Actions;

internal class BackupProcessor : ActionProcessor {
    public BackupProcessor() : base("백업", Config[ShutdownType.Backup]) { }

    protected override void DoProcessCore() {
        File.Delete(BackupDir + VF);
        ProcessDiskpart($"create vdisk file \"{BackupDir}{VF}\" source \"{VHDDir}{VF}\" type expandable");
    }
}