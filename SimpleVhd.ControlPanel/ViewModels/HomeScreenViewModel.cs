using CommunityToolkit.Mvvm.Input;
using SimpleVhd.ControlPanel.Views;

namespace SimpleVhd.ControlPanel.ViewModels;

public sealed partial class HomeScreenViewModel(IWindow window, IScreen screen) : ScreenViewModel {
    [RelayCommand]
    private void BackupButton() {
        var mw = (MainWindow)window;
        mw.ViewModel.Screen = new BackupScreen(window, screen);
    }

    [RelayCommand]
    private void RestoreButton() {

    }
}
