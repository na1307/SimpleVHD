using SimpleVhd.Installer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.Installer.Views;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class InstallerMainWindow {
    public InstallerMainWindow(InstallType installType) {
        InitializeComponent();
        ViewModel = new(installType);
    }

    private InstallerMainWindowViewModel ViewModel { get; }
}
