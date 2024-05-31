using CommunityToolkit.Mvvm.Input;
using SimpleVhd.ControlPanel.Views;

namespace SimpleVhd.ControlPanel.ViewModels;

public sealed partial class HomeScreenViewModel(IWindow window, IScreen screen) : ScreenViewModel {
    [RelayCommand]
    private void BackupButton() {

    }

    [RelayCommand]
    private void RestoreButton() {

    }
}
