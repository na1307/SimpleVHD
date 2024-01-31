using System.Diagnostics;

namespace SimpleVhd.Installer;

public partial class FormMain : Form {
    public FormMain() {
        InitializeComponent();

        if (File.Exists(Path.Combine(Application.StartupPath, "..", SettingsFileName))) {
            button1.Enabled = false;
        }
    }

    private async void button1_Click(object sender, EventArgs e) {
        if (await FormCheckRequirements.CheckAsync(InstallType.NewInstall)) {
            using FormWizard wizard = new([new GetVhdType()]);

            if (wizard.ShowDialog() == DialogResult.OK) {
                await FormInstalling.InstallAsync();
                endProcess();
            }
        }
    }

    private void endProcess() {
        MessageBox.Show("작업을 완료했습니다.", "작업 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Process.Start("ControlPanel.exe");
        Close();
    }
}
