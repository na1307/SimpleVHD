namespace SimpleVHD.PEAction;

public sealed partial class FormMain {
    protected sealed override CreateParams CreateParams {
        get {
            var myCp = base.CreateParams;
            myCp.ClassStyle |= 512;
            return myCp;
        }
    }

    public FormMain() {
        InitializeComponent();
        if (PVConfig.Instance.OperatingStyle == OperatingStyle.Simple) button2.Enabled = false;
        if (!BackupExists) button1.Enabled = false;
    }

    private void button_Click(object sender, EventArgs e) {
        (string text, DoAction action) = ((Button)sender).Name switch {
            nameof(button1) => ("복원", DoAction.DoRestore),
            nameof(button2) => ("초기화", DoAction.DoRevert),
            _ => throw new NotImplementedException()
        };

        if (MessageBox.Show("정말 " + text + " 작업을 실행할까요?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes) {
            Hide();
            Actions.ActionFactory.Create(action).Run();
        }
    }

    private void button0_Click(object sender, EventArgs e) => Application.Exit();

    private void button3_Click(object sender, EventArgs e) {
        FormAdvanced formAdvanced = new();

        if (formAdvanced.ShowDialog() == DialogResult.OK) Hide();
    }

    private void button4_Click(object sender, EventArgs e) {
        using var cmd = Process.Start("cmd.exe"); cmd.WaitForExit();
    }

    private void button9_Click(object sender, EventArgs e) => new FormAbout().ShowDialog();
}