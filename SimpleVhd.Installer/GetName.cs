namespace SimpleVhd.Installer;

public partial class GetName : UserControl, ISetupWizardPage {
    public GetName() => InitializeComponent();

    public string Title => "이름 입력";
    public string Description => "이 VHD의 이름(별명)을 입력해 주세요.";
    public UserControl Panel => this;

    private void textBox1_TextChanged(object sender, EventArgs e) => Status.Processor!.Name = !string.IsNullOrWhiteSpace(textBox1.Text) ? textBox1.Text : InstallProcessor.NoName;
}
