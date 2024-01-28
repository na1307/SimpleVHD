namespace SimpleVhd.Installer;

public partial class FormCheckRequirements : Form {
    public FormCheckRequirements() {
        InitializeComponent();
        Statics.Data = new NewInstallData();
    }

    public void Check() {
        try {
            Statics.Data?.CheckRequirements();
        } finally {
            Close();
        }
    }
}
