using Bluehill.Vhd;

namespace SimpleVhd.PE.Operations;

internal class Restore : Operation {
    public override string OperationName => "복원";

    protected override Task WorkCore() {
        return Task.Run(restore);

        static void restore() {
            var backupFile = Path.Combine(SVPath, BackupDirName, OFile);
            var sourceFile = ODrv + OInstance.Directory + OFile;

            if (!File.Exists(backupFile)) {
                throw new OperationFailedException("백업 파일이 존재하지 않습니다.");
            }

            File.Delete(sourceFile);
            VhdFunctions.CloneVhd(sourceFile, backupFile, default, OInstance.Type == VhdType.Fixed);
        }
    }
}
