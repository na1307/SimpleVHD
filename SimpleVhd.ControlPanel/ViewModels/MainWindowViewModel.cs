using CommunityToolkit.Mvvm.ComponentModel;
using SimpleVhd.ControlPanel.Views;

namespace SimpleVhd.ControlPanel.ViewModels;

public sealed partial class MainWindowViewModel(IWindow window) : ObservableObject {
    [ObservableProperty]
    private Screen screen = new HomeScreen(window);
}
