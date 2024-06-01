using SimpleVhd.ControlPanel.ViewModels;

namespace SimpleVhd.ControlPanel.Views;

public sealed class BackupScreen(IMainWindow window, IScreen previousScreen) : OperationScreen {
    protected override BackupScreenViewModel ViewModel { get; } = new(window, previousScreen);
}
