using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SimpleVhd.ControlPanel.Views;

namespace SimpleVhd.ControlPanel.ViewModels;

public abstract partial class OperationViewModel(IScreen previousScreen) : ScreenViewModel {
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract string Icon { get; }

    [RelayCommand]
    private void BackButton() => Ioc.Default.GetRequiredService<MainViewModel>().Screen = previousScreen.Self;

    [RelayCommand]
    protected abstract void ProcessButton();
}
