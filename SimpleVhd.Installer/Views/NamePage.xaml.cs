// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.Installer.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class NamePage {
    public NamePage(InstallProcessor processor) {
        InitializeComponent();
        Processor = processor;
    }

    public override string Title => "인스턴스 이름";
    public override string Description => "이 VHD 인스턴스의 이름(별명)을 입력해 주세요.";
    private InstallProcessor Processor { get; }
}
