namespace SimpleVhd.PE.Operations;

public class Restore : Operation {
    public override string OperationName => "복원";

    protected override async Task WorkCore() {
        File.Delete(Path.Combine(ODrv, OInstance.Directory, OFile));
        await ProcessDiskpartAsync($"create vdisk file \"{Path.Combine(ODrv, OInstance.Directory, OFile)}\" source \"{Path.Combine(SVPath, BackupDirName, OFile)}\" type {OInstance.Type}");
    }
}
