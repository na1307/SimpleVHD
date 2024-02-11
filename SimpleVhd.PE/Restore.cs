namespace SimpleVhd.PE;

public class Restore : Operation {
    public override string OperationName => "복원";

    protected override void WorkCore() {
        File.Delete(ODrv + OInstance.Directory + OFile);
        ProcessDiskpart($"create vdisk file \"{ODrv}{OInstance.Directory}{OFile}\" source \"{Path.Combine(BackupDir, OFile)}\" type {OInstance.Type}");
    }
}
