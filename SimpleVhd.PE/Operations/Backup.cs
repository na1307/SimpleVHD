using Bluehill.Vhd;

namespace SimpleVhd.PE.Operations;

internal class Backup : Operation {
    public override string OperationName => "백업";

    protected override Task WorkCore() {
        return Task.Run(backup);

        static void backup() {
            var backupFile = Path.Combine(SVPath, BackupDirName, OFile);
            var sourceFile = ODrv + OInstance.Directory + OFile;

            File.Delete(backupFile);
            VhdFunctions.CloneVhd(backupFile, sourceFile).Dispose();
        }
    }
}
