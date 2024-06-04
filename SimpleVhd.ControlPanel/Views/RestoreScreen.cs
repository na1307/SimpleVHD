using SimpleVhd.ControlPanel.ViewModels;

namespace SimpleVhd.ControlPanel.Views;

public sealed class RestoreScreen(IScreen previousScreen) : OperationScreen {
    protected override RestoreViewModel ViewModel { get; } = new(previousScreen);
}
