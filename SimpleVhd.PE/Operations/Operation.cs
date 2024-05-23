using System.Diagnostics;
using System.Text;

namespace SimpleVhd.PE.Operations;

public abstract class Operation {
    protected static readonly Vhd OInstance = Settings.Instance.Instances[Settings.Instance.InstanceToOperationOn!.Value];
    protected static readonly string OFile = $"{OInstance.FileName}.{OInstance.Format.ToString().ToLowerInvariant()}";
    protected static readonly string ODrv = DriveInfo.GetDrives().First(d => File.Exists(Path.Combine(d.Name, OInstance.Directory, OFile))).GetDriveLetterAndColon();
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
            Settings.Instance.InstanceToOperationOn = null;
            Settings.Instance.OperationTempValue = null;
        }
    }

    protected static async Task ProcessDiskpartAsync(params string[] cmds) {
        ArgumentNullException.ThrowIfNull(cmds);

        using (StreamWriter ds = new(dptemp, false, systemEncoding)) {
            Array.ForEach(cmds, ds.WriteLine);
            ds.WriteLine("exit");
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

    protected sealed class DiskpartException : SimpleVhdException {
        public DiskpartException(string message) : base(message) { }
        public DiskpartException(string message, Exception innerException) : base(message, innerException) { }
    }
}
