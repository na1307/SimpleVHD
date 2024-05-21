// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel.Installer;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class NamePage {
    private readonly InstallInput input;

    public NamePage(InstallInput input) {
        InitializeComponent();
        this.input = input;
    }

    public override string Title => "제목";
    public override string Description => "이건 설명";
}
