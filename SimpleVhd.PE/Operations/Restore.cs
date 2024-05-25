namespace SimpleVhd.PE.Operations;

public class Restore : Operation {
    public override string OperationName => "복원";

    protected override async Task WorkCore() {
        var backupFile = Path.Combine(SVPath, BackupDirName, OFile);
        var sourceFile = Path.Combine(ODrv, OInstance.Directory, OFile);

        if (!File.Exists(backupFile)) {
            throw new OperationFailedException("백업 파일이 존재하지 않습니다.");
        }

        File.Delete(sourceFile);
        await ProcessDiskpartAsync($"create vdisk file \"{sourceFile}\" source \"{backupFile}\" type {OInstance.Type}");
    }
}
