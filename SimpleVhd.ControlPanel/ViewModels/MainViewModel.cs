using CommunityToolkit.Mvvm.ComponentModel;
using SimpleVhd.ControlPanel.Views;

namespace SimpleVhd.ControlPanel.ViewModels;

public sealed partial class MainViewModel : ObservableObject {
    [ObservableProperty]
    private Screen screen = new HomeScreen();
}
