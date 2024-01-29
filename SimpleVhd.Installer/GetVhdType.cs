namespace SimpleVhd.Installer;

public partial class GetVhdType : UserControl, ISetupWizardPage {
    public GetVhdType() => InitializeComponent();

    public string Title => "VHD 형식 선택";
    public string Description => "현재 VHD의 형식을 선택해주세요.";
    public UserControl Panel => this;

    private void radioButton1_CheckedChanged(object sender, EventArgs e) => Status.Processor!.Type = VhdType.Fixed;
    private void radioButton2_CheckedChanged(object sender, EventArgs e) => Status.Processor!.Type = VhdType.Expandable;
}
