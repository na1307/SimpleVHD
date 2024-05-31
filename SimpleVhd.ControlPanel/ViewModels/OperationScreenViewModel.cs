using CommunityToolkit.Mvvm.Input;
using SimpleVhd.ControlPanel.Views;

namespace SimpleVhd.ControlPanel.ViewModels;

public abstract partial class OperationScreenViewModel(IWindow window, IScreen previousScreen) : ScreenViewModel {
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract string Icon { get; }

    [RelayCommand]
    private void BackButton() {
        var mw = (MainWindow)window;
        mw.ViewModel.Screen = (Screen)previousScreen;
    }

    [RelayCommand]
    protected abstract void ProcessButton();
}
