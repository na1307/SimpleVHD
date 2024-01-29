namespace SimpleVhd.Installer;

public partial class FormCheckRequirements : Form {
    public FormCheckRequirements(InstallType type) {
        InitializeComponent();

        Status.Processor = type switch {
            InstallType.NewInstall => new NewInstallProcessor(),
            _ => throw new ArgumentException("No", nameof(type)),
        };
    }

    public void Check() {
        try {
            Status.Processor?.CheckRequirements();
        } finally {
            Close();
        }
    }
}
