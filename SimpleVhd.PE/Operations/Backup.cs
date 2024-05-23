namespace SimpleVhd.PE.Operations;

public class Backup : Operation {
    public override string OperationName => "백업";

    protected override async Task WorkCore() {
        File.Delete(Path.Combine(SVPath, BackupDirName, OFile));
        await ProcessDiskpartAsync($"create vdisk file \"{Path.Combine(SVPath, BackupDirName, OFile)}\" source \"{Path.Combine(ODrv, OInstance.Directory, OFile)}\" type expandable");
    }
}
