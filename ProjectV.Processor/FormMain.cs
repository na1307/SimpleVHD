#nullable enable
using ProjectV.Processor.Actions;

namespace ProjectV.Processor;

public partial class FormMain {
    protected sealed override CreateParams CreateParams {
        get {
            var myCp = base.CreateParams;
            myCp.ClassStyle |= 512;
            return myCp;
        }
    }

    public FormMain() => InitializeComponent();

    private void FormMain_Load(object sender, EventArgs e) {
        try {
            if (string.IsNullOrEmpty(PVDir)) throw new PVDirectoryNotFoundException();
            if (string.IsNullOrEmpty(BackupDir)) throw new BackupDirectoryNotFoundException();
            if (string.IsNullOrEmpty(VHDDir)) throw new VhdFileNotFoundException();

            var config = PVConfig.Instance;

            switch (config.OperatingStyle) {
                case OperatingStyle.Simple:
                    button2.Enabled = false;
                    break;

                case OperatingStyle.DifferentialManual:
                    break;

                case OperatingStyle.DifferentialAuto:
                    break;

                default:
                    throw new PVConfig.InvalidConfigException("OperatingStyle이 잘못되었습니다.");
            }

            switch (config.VhdType) {
                case VhdType.Fixed:
                case VhdType.Expandable: break;
                default: throw new PVConfig.InvalidConfigException("VhdType이 잘못되었습니다.");
            }

            Application.ApplicationExit += (s, e) => {
                var config = PVConfig.Instance;
                config.Action = DoAction.DoNothing;
                config.SaveConfig();
            };

            switch (config.Action) {
                case DoAction.DoNothing:
                    break;

                case DoAction.DoBackup:
                    new BackupProcessor().DoProcess();
                    break;

                case DoAction.DoRestore:
                    new RestoreProcessor().DoProcess();
                    break;

                case DoAction.DoRevert:
                    new RevertProcessor().DoProcess();
                    break;

                case DoAction.DoMerge:
                    new MergeProcessor().DoProcess();
                    break;

                case DoAction.DoExpand:
                    new ExpandProcessor().DoProcess();
                    break;

                case DoAction.DoConvertType:
                    new ConvertTypeProcessor().DoProcess();
                    break;

                case DoAction.DoConvertFormat:
                    new ConvertFormatProcessor().DoProcess();
                    break;

                case DoAction.DoSwitchStyle:
                    new SwitchStyleProcessor().DoProcess();
                    break;

                case DoAction.DoRebuild:
                    new RebuildProcessor().DoProcess();
                    break;

                case DoAction.DoParentBoot:
                case DoAction.DoUninstall:
                    throw new InvalidOperationException();

                default:
                    throw new PVConfig.InvalidConfigException("Action이 잘못되었습니다.");
            }

            if (!File.Exists(BackupDir + config.VhdFile)) button1.Enabled = false;
        } catch (PVProcessorException ex) {
            ErrMsg(ex.Message, true);
        } catch (Exception ex) {
            ErrMsg(ex.ToString(), true);
        }
    }

    private void button0_Click(object sender, EventArgs e) => Application.Exit();

    private void button1_Click(object sender, EventArgs e) {
        if (MessageBox.Show("정말 복원 작업을 실행할까요?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes) {
            Hide();
            new RestoreProcessor().DoProcess();
        }
    }

    private void button2_Click(object sender, EventArgs e) {
        if (MessageBox.Show("정말 초기화 작업을 실행할짜요?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes) {
            Hide();
            new RevertProcessor().DoProcess();
        }
    }

    private void button3_Click(object sender, EventArgs e) {
        FormAdvanced formAdvanced = new();

        if (formAdvanced.ShowDialog() == DialogResult.OK) Hide();
    }

    private void button4_Click(object sender, EventArgs e) {
        using var cmd = Process.Start("cmd.exe"); cmd.WaitForExit();
    }

    private void button9_Click(object sender, EventArgs e) => new FormAbout().ShowDialog();
}