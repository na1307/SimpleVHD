using SimpleVhd.ControlPanel.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel.Views;

public sealed class BackupScreen(IWindow window, IScreen previousScreen) : OperationScreen {
    protected override BackupScreenViewModel ViewModel { get; } = new(window, previousScreen);
}
