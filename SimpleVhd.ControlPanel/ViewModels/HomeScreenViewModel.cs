using CommunityToolkit.Mvvm.Input;
using SimpleVhd.ControlPanel.Views;

namespace SimpleVhd.ControlPanel.ViewModels;

public sealed partial class HomeScreenViewModel(IMainWindow window, IScreen screen) : ScreenViewModel {
    [RelayCommand]
    private void BackupButton() => window.ViewModel.Screen = new BackupScreen(window, screen);

    [RelayCommand]
    private void RestoreButton() => window.ViewModel.Screen = new RestoreScreen(window, screen);
}
