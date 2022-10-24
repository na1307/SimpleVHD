namespace SimpleVHD.PEAction.Actions;

internal abstract class Action {
    private const string dptemp = "dptemp.txt";
    private readonly Form ipForm;

    /// <summary>
    /// 작업의 이름
    /// </summary>
    protected abstract string Name { get; }

    /// <summary>
    /// 작업이 차등 스타일에서만 지원되는지 여부
    /// </summary>
    protected virtual bool DifferentialOnly => false;

    /// <summary>
    /// 작업을 진행하기 위해 백업이 필요한지 여부
    /// </summary>
    protected virtual bool NeedBackup => false;

    /// <summary>
    /// 작업 후 깨끗한 자식을 재구축할지 여부
    /// </summary>
    protected virtual bool AfterRebuild => false;

    /// <summary>
    /// 작업 후 디스크를 초기화할지 여부
    /// </summary>
    protected virtual bool AfterRevert => false;

    /// <summary>
    /// 작업을 완료한 후 설정 파일의 Temp 항목을 제거해야 하는지 여부
    /// </summary>
    protected virtual bool RemoveTempAfterProcess => false;

    /// <summary>
    /// 작업 후 다시 시작하는 대신 종료할지 여부
    /// </summary>
    protected virtual bool Shutdown => false;

    protected Action() {
        ipForm = new() {
            AutoScaleMode = AutoScaleMode.None,
            ClientSize = new(300, 50),
            Font = new("맑은 고딕", 9.0f),
            FormBorderStyle = FormBorderStyle.None,
            ShowInTaskbar = false,
            StartPosition = FormStartPosition.CenterScreen,
            TopLevel = true,
            TopMost = true
        };

        Label l = new() {
            AutoEllipsis = true,
            Dock = DockStyle.Fill,
            Text = Name + " 작업이 진행 중입니다. 잠시 기다려 주세요...",
            TextAlign = ContentAlignment.MiddleCenter
        };

        ipForm.Controls.Add(l);

        ipForm.Disposed += (sender, e) => l.Dispose();
    }

    public sealed override string? ToString() => null;

    /// <summary>
    /// 작업을 실행함
    /// </summary>
    public void Run() {
        try {
            ipForm.Show();
            if (DifferentialOnly && !IsDifferentialStyle) throw new ProcessFailedException("단순 스타일에서는 " + Name + " 작업이 지원되지 않습니다.");
            if (NeedBackup && !File.Exists(BackupDir + PVConfig.Instance.VhdFile)) throw new ProcessFailedException("백업 파일을 찾을 수 없습니다.");
            RunCore();

            if (IsDifferentialStyle) {
                if (AfterRebuild) {
                    File.Delete(VhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower());
                    ProcessDiskpart($"create vdisk file \"{VhdDir}{ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower()}\" parent \"{VhdDir}{PVConfig.Instance.VhdFile}\"");
                }

                if (AfterRevert) {
                    File.Delete(VhdDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower());
                    File.Delete(VhdDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower());
                    File.Copy(VhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), VhdDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);
                    File.Copy(VhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), VhdDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);
                }
            }
        } catch (PVActionException ex) {
            ErrMsg(ex.Message, true);
        } catch (Exception ex) {
            ErrMsg(ex.ToString(), true);
        } finally {
            if (RemoveTempAfterProcess) PVConfig.Instance.Temp = null;
            ipForm.Close();
            Environment.ExitCode = !Shutdown ? RestartCode : ShutdownCode;
            Application.Exit();
        }
    }

    protected virtual void RunCore() { }

    protected static void ProcessDiskpart(params string[] cmds) {
        if (cmds == null) throw new ArgumentNullException(nameof(cmds));

        using (StreamWriter ds = new(PVDir + dptemp, false, System.Text.Encoding.GetEncoding(949))) {
            cmds.ForEach(ds.WriteLine);
            ds.WriteLine("exit");
        }

        using (Process diskpart = new() {
            StartInfo = {
                FileName = "diskpart.exe",
                Arguments = $"/s \"{PVDir}{dptemp}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            }
        }) {
            diskpart.Start();
            diskpart.WaitForExit();

            File.Delete(PVDir + dptemp);

            if (diskpart.ExitCode != 0) throw new ProcessFailedException("diskpart 작업이 실패했습니다. 종료 코드는 " + diskpart.ExitCode + "입니다.");
        }
    }

    protected static string ProcessDiskpartOutput(params string[] cmds) {
        if (cmds == null) throw new ArgumentNullException(nameof(cmds));

        using (StreamWriter ds = new(PVDir + dptemp, false, System.Text.Encoding.GetEncoding(949))) {
            cmds.ForEach(ds.WriteLine);
            ds.WriteLine("exit");
        }

        using (Process diskpart = new() {
            StartInfo = {
                FileName = "cmd.exe",
                Arguments = $"/c diskpart /s \"{PVDir}{dptemp}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        }) {
            diskpart.Start();
            diskpart.WaitForExit();

            File.Delete(PVDir + dptemp);

            return diskpart.ExitCode == 0 ? diskpart.StandardOutput.ReadToEnd() : throw new ProcessFailedException("diskpart 작업이 실패했습니다. 종료 코드는 " + diskpart.ExitCode + "입니다.");
        }
    }

    protected sealed class ProcessFailedException : PVActionException {
        private const string dMessage = "작업이 실패했습니다.\r\n\r\n";

        public ProcessFailedException(string reason) : base(dMessage + reason) { }
        public ProcessFailedException(string reason, Exception innerException) : base(dMessage + reason, innerException) { }
    }
}