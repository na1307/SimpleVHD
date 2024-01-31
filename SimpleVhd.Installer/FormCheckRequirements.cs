namespace SimpleVhd.Installer;

public partial class FormCheckRequirements : Form {
    private FormCheckRequirements(InstallType type) {
        InitializeComponent();

        Status.Processor = type switch {
            InstallType.NewInstall => new NewInstallProcessor(),
            _ => throw new ArgumentException("No", nameof(type)),
        };
    }

    public static async Task CheckAsync(InstallType installType) {
        using FormCheckRequirements form = new(installType);
        Task task = form.check();
        form.ShowDialog();
        await task;
    }

    private async Task check() {
        try {
            await Task.Run(() => Status.Processor!.CheckRequirements());
        } finally {
            Close();
        }
    }
}
