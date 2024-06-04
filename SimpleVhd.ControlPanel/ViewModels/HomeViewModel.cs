using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SimpleVhd.ControlPanel.Views;

namespace SimpleVhd.ControlPanel.ViewModels;

public sealed partial class HomeViewModel(IScreen screen) : ScreenViewModel {
    private const string template = "현재 {0} 관리 중";

    [ObservableProperty]
    private string current = string.Format(template, Settings.Instance.CurrentInstance!.Name);

    [RelayCommand]
    private async Task RenameButtonAsync() {
        RenameDialogContent content = new();
        ContentDialog dialog = new() {
            XamlRoot = screen.XamlRoot,
            Title = "이름 바꾸기",
            PrimaryButtonText = "확인",
            CloseButtonText = "취소",
            DefaultButton = ContentDialogButton.Primary,
            Content = content
        };

        var result = await dialog.ShowAsync();

        if (result is ContentDialogResult.Primary) {
            Settings.Instance.CurrentInstance!.Name = content.ViewModel.Name;
            Current = string.Format(template, Settings.Instance.CurrentInstance!.Name);
        }
    }

    [RelayCommand]
    private void BackupButton() => Ioc.Default.GetRequiredService<MainViewModel>().Screen = new BackupScreen(screen);

    [RelayCommand]
    private void RestoreButton() => Ioc.Default.GetRequiredService<MainViewModel>().Screen = new RestoreScreen(screen);
}
