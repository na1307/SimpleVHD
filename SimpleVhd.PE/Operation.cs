using System.Diagnostics;

namespace SimpleVhd.PE;

public abstract class Operation {
    protected static readonly Vhd OInstance = Settings.Instance.VhdInstances[Settings.Instance.InstanceToOperationOn!.Value];
    protected static readonly string OFile = $"{OInstance.ParentFile}.{OInstance.Format.ToString().ToLowerInvariant()}";
    protected static readonly string ODrv = DriveInfo.GetDrives().First(d => File.Exists(d.Name.TrimEnd('\\') + OInstance.Directory + OFile)).Name.TrimEnd('\\');
    private static readonly string dptemp = Path.Combine(SVDir, "dptemp.txt");

    public abstract string OperationName { get; }

    public Task WorkAsync() {
        return Task.Run(workInternal);

        void workInternal() {
            try {
                WorkCore();
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
    }

    protected static void ProcessDiskpart(params string[] cmds) {
        ArgumentNullException.ThrowIfNull(cmds);

        using (StreamWriter ds = new(dptemp, false, SystemEncoding)) {
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
        diskpart.WaitForExit();

        File.Delete(dptemp);

        if (diskpart.ExitCode != 0) {
            throw new SimpleVhdException("diskpart 작업이 실패했습니다. 종료 코드는 " + diskpart.ExitCode + "입니다.");
        }
    }

    protected abstract void WorkCore();
}
