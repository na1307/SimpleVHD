using SimpleVhd.ControlPanel.ViewModels;

namespace SimpleVhd.ControlPanel.Views;

public sealed class BackupScreen(IScreen previousScreen) : OperationScreen {
    protected override BackupViewModel ViewModel { get; } = new(previousScreen);
}
