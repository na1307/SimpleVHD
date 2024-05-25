namespace SimpleVhd.PE.Operations;

public class Backup : Operation {
    public override string OperationName => "백업";

    protected override async Task WorkCore() {
        var backupFile = Path.Combine(SVPath, BackupDirName, OFile);
        var sourceFile = Path.Combine(ODrv, OInstance.Directory, OFile);

        File.Delete(backupFile);
        await ProcessDiskpartAsync($"create vdisk file \"{backupFile}\" source \"{sourceFile}\" type expandable");
    }
}
