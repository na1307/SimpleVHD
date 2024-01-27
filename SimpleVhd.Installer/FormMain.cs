namespace SimpleVhd.Installer;

public partial class FormMain : Form {
    public FormMain() => InitializeComponent();

    private void button1_Click(object sender, EventArgs e) {
        FormCheckRequirements check = new();

        check.Show(this);

        try {
            check.Check();
        } catch (RequirementsNotMetException ex) {
            MessageBox.Show("요구 사항이 맞지 않습니다." + Environment.NewLine + Environment.NewLine + ex.Message);
            return;
        }

        using FormWizard wizard = new([new GetVhdType()]);

        if (wizard.ShowDialog(this) == DialogResult.OK) {
            Statics.Data!.InstallProcess();
            MessageBox.Show("설치 성공!");
        }
    }
}
