using CommunityToolkit.Mvvm.ComponentModel;
using SimpleVhd.ControlPanel.Views;

namespace SimpleVhd.ControlPanel.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject {
    [ObservableProperty]
    private Screen screen = new HomeScreen();
}
