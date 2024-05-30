using System.ComponentModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel.Views;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : INotifyPropertyChanged {
    private Screen _screen = new HomeScreen();

    public MainWindow() => InitializeComponent();

    public event PropertyChangedEventHandler? PropertyChanged;

    public Screen Screen {
        get => _screen;
        set {
            _screen = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Screen)));
        }
    }
}
