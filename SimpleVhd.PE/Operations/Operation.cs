using System.Diagnostics;
using System.Text;

namespace SimpleVhd.PE.Operations;

public abstract class Operation {
    protected static readonly Vhd OInstance = Settings.Instance.Instances[Settings.Instance.OperationTarget!.Value];
    protected static readonly string OFile = $"{OInstance.FileName}.{OInstance.Format.ToString().ToLowerInvariant()}";
    protected static readonly string ODrv =
        DriveInfo.GetDrives().First(d => File.Exists(d.GetDriveLetterAndColon() + OInstance.Directory + OFile)).GetDriveLetterAndColon();
    private static readonly string dptemp = Path.Combine(SVPath, "dptemp.txt");
    private static readonly Encoding systemEncoding = CodePagesEncodingProvider.Instance.GetEncoding(0)!;

    public abstract string OperationName { get; }

    public async Task WorkAsync() {
        try {
            await WorkCore();
        } catch (SimpleVhdException ex) {
            ErrMsg(ex.Message);
        } catch (Exception ex) {
            ErrMsg(ex.ToString());
        } finally {
            Settings.Instance.OperationType = null;
            Settings.Instance.OperationTarget = null;
            Settings.Instance.OperationTempValue = null;
        }
    }

    protected static async Task ProcessDiskpartAsync(params string[] cmds) {
        ArgumentNullException.ThrowIfNull(cmds);

        await using (StreamWriter ds = new(dptemp, false, systemEncoding)) {
            foreach (var cmd in cmds) {
                await ds.WriteLineAsync(cmd);
            }

            await ds.WriteLineAsync("exit");
        }

        using Process diskpart = new() {
            StartInfo = {
                FileName = "diskpart.exe",
                Arguments = "/s \"" + dptemp + "\"",
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        diskpart.Start();
        await diskpart.WaitForExitAsync();

        File.Delete(dptemp);

        if (diskpart.ExitCode != 0) {
            throw new DiskpartException("diskpart 작업이 실패했습니다. 종료 코드는 " + diskpart.ExitCode + "입니다.");
        }
    }

    protected abstract Task WorkCore();

    protected class OperationFailedException : SimpleVhdException {
        public OperationFailedException(string message) : base(message) { }
        public OperationFailedException(string message, Exception innerException) : base(message, innerException) { }
    }

    private sealed class DiskpartException : OperationFailedException {
        public DiskpartException(string message) : base(message) { }
        public DiskpartException(string message, Exception innerException) : base(message, innerException) { }
    }
}
