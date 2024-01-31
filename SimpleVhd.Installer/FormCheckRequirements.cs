namespace SimpleVhd.Installer;

public partial class FormCheckRequirements : FormWorkingDialog {
    private FormCheckRequirements(InstallType type) {
        InitializeComponent();

        Status.Processor = type switch {
            InstallType.NewInstall => new NewInstallProcessor(),
            _ => throw new ArgumentException("No", nameof(type)),
        };
    }

    public static async Task<bool> CheckAsync(InstallType installType) {
        try {
            using FormCheckRequirements form = new(installType);
            Task task = form.check();
            form.ShowDialog();
            await task;
            return true;
        } catch (RequirementsNotMetException ex) {
            ErrMsg("요구 사항이 맞지 않습니다." + Environment.NewLine + Environment.NewLine + ex.Message);
            return false;
        }
    }

    private async Task check() {
        try {
            await Task.Run(() => Status.Processor!.CheckRequirements());
        } finally {
            Close();
        }
    }
}
