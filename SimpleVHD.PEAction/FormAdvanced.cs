using SimpleVHD.PEAction.Actions;

namespace SimpleVHD.PEAction;

public partial class FormAdvanced {
    public FormAdvanced() {
        InitializeComponent();
        OK_Button.Visible = false;

        if (!BackupExists) {
            button2.Enabled = false;
        }
    }

    private void button1_Click(object sender, EventArgs e) => start<Rebuild>("재구축");
    private void button2_Click(object sender, EventArgs e) => start<EmergencyRestore>("응급 복원");
    private void button3_Click(object sender, EventArgs e) => start<SwitchSimpleStyle>("단순 스타일 전환");

    private void start<TAction>(string name) where TAction : Actions.Action, new() {
        if (MessageBox.Show("정말 " + name + " 작업을 실행할까요?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes) {
            base.OK_Button_Click(null, EventArgs.Empty);
            new TAction().Run();
        }
    }
}