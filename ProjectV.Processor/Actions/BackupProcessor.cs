#nullable enable
namespace ProjectV.Processor.Actions;

internal class BackupProcessor : ActionProcessor {
    public BackupProcessor() : base("백업", PVConfig.Instance[ShutdownType.Backup]) { }

    protected override void DoProcessCore() {
        File.Delete(BackupDir + PVConfig.Instance.VhdFile);
        ProcessDiskpart($"create vdisk file \"{BackupDir}{PVConfig.Instance.VhdFile}\" source \"{VhdDir}{PVConfig.Instance.VhdFile}\" type expandable");
    }
}