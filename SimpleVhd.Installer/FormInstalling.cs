namespace SimpleVhd.Installer;

public partial class FormInstalling : FormWorkingDialog {
    private FormInstalling() => InitializeComponent();

    public static async Task InstallAsync() {
        using FormInstalling fi = new();
        Task task = fi.install();
        fi.ShowDialog();
        await task;
    }

    private async Task install() {
        try {
            await Task.Run(() => Status.Processor!.InstallProcess());
        } finally {
            Close();
        }
    }
}
