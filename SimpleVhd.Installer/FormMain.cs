using System.Diagnostics;

namespace SimpleVhd.Installer;

public partial class FormMain : Form {
    public FormMain() {
        InitializeComponent();

        if (File.Exists(Path.Combine(Application.StartupPath, "..", SettingsFileName))) {
            button1.Enabled = false;
        }
    }

    private void button1_Click(object sender, EventArgs e) {
        checkRequirements(new FormCheckRequirements(InstallType.NewInstall));

        using FormWizard wizard = new([new GetVhdType()]);

        if (wizard.ShowDialog(this) == DialogResult.OK) {
            Status.Processor?.InstallProcess();
            MessageBox.Show("설치 성공!", "설치", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.Start("ControlPanel.exe");
            Close();
        }
    }

    private static void checkRequirements(FormCheckRequirements form) {
        form.Show();

        try {
            form.Check();
        } catch (RequirementsNotMetException ex) {
            ErrMsg("요구 사항이 맞지 않습니다." + Environment.NewLine + Environment.NewLine + ex.Message);
        }
    }
}
