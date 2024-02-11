namespace SimpleVhd.PE;

public class Backup : Operation {
    public override string OperationName => "백업";

    protected override void WorkCore() {
        File.Delete(Path.Combine(BackupDir, OFile));
        ProcessDiskpart($"create vdisk file \"{Path.Combine(BackupDir, OFile)}\" source \"{ODrv}{OInstance.Directory}{OFile}\" type expandable");
    }
}
