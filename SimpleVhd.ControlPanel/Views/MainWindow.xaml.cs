using SimpleVhd.ControlPanel.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel.Views;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : IMainWindow {
    public MainWindow() {
        InitializeComponent();
        ViewModel = new(this);
    }

    public MainWindowViewModel ViewModel { get; }
}
