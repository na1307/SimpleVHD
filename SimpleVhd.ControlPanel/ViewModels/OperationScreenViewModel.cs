using CommunityToolkit.Mvvm.Input;
using SimpleVhd.ControlPanel.Views;

namespace SimpleVhd.ControlPanel.ViewModels;

public abstract partial class OperationScreenViewModel(IMainWindow window, IScreen previousScreen) : ScreenViewModel {
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract string Icon { get; }

    [RelayCommand]
    private void BackButton() => window.ViewModel.Screen = previousScreen.Self;

    [RelayCommand]
    protected abstract void ProcessButton();
}
