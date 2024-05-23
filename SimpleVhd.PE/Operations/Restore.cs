namespace SimpleVhd.PE.Operations;

public class Restore : Operation {
    public override string OperationName => "복원";

    protected override async Task WorkCore() {
        if (!File.Exists(Path.Combine(SVPath, BackupDirName, OFile))) {
            throw new OperationFailedException("백업 파일이 존재하지 않습니다.");
        }

        File.Delete(Path.Combine(ODrv, OInstance.Directory, OFile));
        await ProcessDiskpartAsync($"create vdisk file \"{Path.Combine(ODrv, OInstance.Directory, OFile)}\" source \"{Path.Combine(SVPath, BackupDirName, OFile)}\" type {OInstance.Type}");
    }
}
