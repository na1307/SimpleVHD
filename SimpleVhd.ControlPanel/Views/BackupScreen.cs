using SimpleVhd.ControlPanel.ViewModels;

namespace SimpleVhd.ControlPanel.Views;

public sealed class BackupScreen(IWindow window, IScreen previousScreen) : OperationScreen {
    protected override BackupScreenViewModel ViewModel { get; } = new(window, previousScreen);
}
