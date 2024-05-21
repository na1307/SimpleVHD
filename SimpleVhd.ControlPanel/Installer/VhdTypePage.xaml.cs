// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel.Installer;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class VhdTypePage {
    private readonly InstallInput input;

    public VhdTypePage(InstallInput input) {
        InitializeComponent();
        this.input = input;
    }

    public override string Title => "테스트 페이지";
    public override string Description => "또 다른 테스트 페이지";
}
