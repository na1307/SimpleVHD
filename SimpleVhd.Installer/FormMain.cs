namespace SimpleVhd.Installer;

public partial class FormMain : Form {
    public FormMain() {
        InitializeComponent();

        if (File.Exists(Path.Combine(Application.StartupPath, "..", SettingsFileName))) {
            button1.Enabled = false;
        }
    }

    private async void button1_Click(object sender, EventArgs e) {
        try {
            await FormCheckRequirements.CheckAsync(InstallType.NewInstall);
        } catch (RequirementsNotMetException ex) {
            ErrMsg("요구 사항이 맞지 않습니다." + Environment.NewLine + Environment.NewLine + ex.Message);
            return;
        }

        await FormWizard.RunWizard([new GetVhdType()]);
        Close();
    }
}
