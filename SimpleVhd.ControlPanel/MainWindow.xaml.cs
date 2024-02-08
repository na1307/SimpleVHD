using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using System.Diagnostics;
using static Bluehill.AssemblyProperties;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : INotifyPropertyChanged {
    private Screen screen = new HomeScreen();

    public MainWindow() => InitializeComponent();

    public event PropertyChangedEventHandler? PropertyChanged;

    public Screen Screen {
        get => screen;
        set {
            screen = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Screen)));
        }
    }

    private void HomeButton_Click(object sender, RoutedEventArgs e) => Screen = new HomeScreen();

    private void ToolsButton_Click(object sender, RoutedEventArgs e) {
        throw new NotImplementedException();
    }

    private void OptionButton_Click(object sender, RoutedEventArgs e) {
        throw new NotImplementedException();
    }

    private void HelpButton_Click(object sender, RoutedEventArgs e) {
        new Process() {
            StartInfo = {
                FileName = "https://na1307.github.io/SimpleVHD/",
                UseShellExecute = true
            }
        }.Start();
    }

    private async void AboutButton_Click(object sender, RoutedEventArgs e) {
        await new ContentDialog() {
            Title = AssemblyTitle + " 정보",
            Content = $"{AssemblyProduct}{Environment.NewLine}{Environment.NewLine}버전 {AssemblyInformationalVersion} (빌드 {BuildNumber}){Environment.NewLine}{Environment.NewLine}{AssemblyCopyright}",
            CloseButtonText = "확인",
            XamlRoot = Content.XamlRoot,
        }.ShowAsync();
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e) => Close();
}
