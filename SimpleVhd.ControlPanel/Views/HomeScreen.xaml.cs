using SimpleVhd.ControlPanel.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel.Views;

public sealed partial class HomeScreen {
    public HomeScreen(IMainWindow window) {
        InitializeComponent();
        ViewModel = new(window, this);
    }

    private HomeScreenViewModel ViewModel { get; }
}
