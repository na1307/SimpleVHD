using PostSharp.Patterns.Model;

namespace SimpleVHD.ControlPanel;

/// <summary>
/// MainWindow.xaml에 대한 상호 작용 논리
/// </summary>
[NotifyPropertyChanged]
public sealed partial class MainWindow {
    public Screen Screen { get; set; } = new HomeScreen();

    public MainWindow() {
        InitializeComponent();
        AboutButton.ToolTip = AssemblyProperties.AssemblyTitle + (string)AboutButton.ToolTip;
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
        switch (((Button)sender).Name) {
            case nameof(HomeButton):
                Screen = new HomeScreen();
                break;

            case nameof(ToolsButton):
                Screen = new ToolsScreen();
                break;

            case nameof(OptionButton):
                Screen = new OptionsScreen();
                break;

            case nameof(HelpButton):
                Process.Start("https://na1307.github.io/SimpleVHD/");
                break;

            case nameof(AboutButton):
                new AboutWindow() { Owner = this }.ShowDialog();
                break;

            case nameof(ExitButton):
                Application.Current.Shutdown();
                break;

            default:
                throw new InvalidOperationException();
        }
    }
}