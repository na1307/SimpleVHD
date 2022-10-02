#nullable enable
using ProjectV.Processor.Actions;

namespace ProjectV.Processor;

public partial class FormAdvanced {
    public FormAdvanced() {
        InitializeComponent();
        OK_Button.Visible = false;

        if (!BackupExists) {
            button2.Enabled = false;
        }
    }

    private void button1_Click(object sender, EventArgs e) {
        if (MessageBox.Show("정말 재구축 작업을 실행할까요?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes) {
            base.OK_Button_Click(sender, e);
            new RebuildProcessor().DoProcess();
        }
    }

    private void button2_Click(object sender, EventArgs e) {
        if (MessageBox.Show("정말 응급 복원 작업을 실행할까요?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes) {
            base.OK_Button_Click(sender, e);
            new EmergencyRestoreProcessor().DoProcess();
        }
    }

    private void button3_Click(object sender, EventArgs e) {
        if (MessageBox.Show("정말 단순 스타일 전환 작업을 실행할까요?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes) {
            PVConfig.Instance.Temp = OperatingStyle.Simple.ToString();
            base.OK_Button_Click(sender, e);
            new SwitchStyleProcessor().DoProcess();
        }
    }
}