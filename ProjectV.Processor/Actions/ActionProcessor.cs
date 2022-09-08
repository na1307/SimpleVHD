#nullable enable
using System.Drawing;

namespace ProjectV.Processor.Actions;

internal abstract class ActionProcessor {
    protected static readonly PVConfig Config = PVConfig.Instance; // Shortcut
    protected static readonly string VF = PVConfig.Instance.VhdFile; // Shortcut
    private const string dptemp = "dptemp.txt";
    private readonly string operationName;
    private readonly bool shutdownCondition;
    private readonly Form ipForm;

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

    protected ActionProcessor(string operation) : this(operation, false) { }

    protected ActionProcessor(string operation, bool shutdown) {
        if (string.IsNullOrWhiteSpace(operation)) throw new ArgumentException($"'{nameof(operation)}'은(는) null이거나 공백일 수 없습니다.", nameof(operation));

        operationName = operation;
        shutdownCondition = shutdown;

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
            Text = operationName + " 작업이 진행 중입니다. 잠시 기다려 주세요...",
            TextAlign = ContentAlignment.MiddleCenter
        };
        ipForm.Controls.Add(l);

        ipForm.Disposed += (sender, e) => l.Dispose();
    }

    public sealed override string? ToString() => null;

    /// <summary>
    /// 작업을 실행함
    /// </summary>
    public void DoProcess() {
        try {
            ipForm.Show();
            if (DifferentialOnly && !IsDifferentialStyle) throw new ProcessFailedException("단순 스타일에서는 " + operationName + " 작업이 지원되지 않습니다.");
            if (NeedBackup && !File.Exists(BackupDir + VF)) throw new ProcessFailedException("백업 파일을 찾을 수 없습니다.");
            DoProcessCore();
        } catch (PVProcessorException ex) {
            ErrMsg(ex.Message, true);
        } catch (Exception ex) {
            ErrMsg(ex.ToString(), true);
        } finally {
            if (IsDifferentialStyle) {
                if (AfterRebuild) {
                    File.Delete(VHDDir + ChildCName);
                    ProcessDiskpart($"create vdisk file \"{VHDDir}{ChildCName}\" parent \"{VHDDir}{VF}\"");
                }

                if (AfterRevert) {
                    File.Delete(VHDDir + Child1Name);
                    File.Delete(VHDDir + Child2Name);
                    File.Copy(VHDDir + ChildCName, VHDDir + Child1Name, true);
                    File.Copy(VHDDir + ChildCName, VHDDir + Child2Name, true);
                }
            }

            if (RemoveTempAfterProcess) Config.Temp = null;
            ipForm.Close();
        }

        Environment.ExitCode = !shutdownCondition ? RestartCode : ShutdownCode;
        Application.Exit();
    }

    protected virtual void DoProcessCore() { }

    protected void ProcessDiskpart(params string[] cmds) {
        if (cmds == null) throw new ArgumentNullException(nameof(cmds));

        using (StreamWriter ds = new(PVDir + dptemp, false, System.Text.Encoding.GetEncoding(949))) {
            foreach (var cmd in cmds) ds.WriteLine(cmd);

            ds.WriteLine("exit");
        }

        using (Process diskpart = new() {
            StartInfo = new() {
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

    protected sealed class ProcessFailedException : PVProcessorException {
        private const string dMessage = "작업이 실패했습니다.\r\n\r\n";

        public ProcessFailedException(string reason) : base(dMessage + reason) { }
        public ProcessFailedException(string reason, Exception innerException) : base(dMessage + reason, innerException) { }
    }
}