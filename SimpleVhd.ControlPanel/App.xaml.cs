using Microsoft.UI.Xaml;
using SimpleVhd.Installer.Models;
using SimpleVhd.Installer.Views;
using WinRT.Interop;
using WinUIEx;
using static SimpleVhd.ControlPanel.NativeMethods;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public sealed partial class App {
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App() {
        InitializeComponent();
        UnhandledException += App_UnhandledException;
    }

    public static new App Current => (App)Application.Current;

    public Window? MWindow { get; private set; }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override async void OnLaunched(LaunchActivatedEventArgs args) {
        Checker.CheckSvPath();
        Checker.CheckSystemVhd();

        if (File.Exists(Path.Combine(SVPath, SettingsFileName))) {
            await Checker.CheckSettingsJsonAsync();

            var settings = Settings.Instance;

            if (settings.OperationType is not null) {
                throw new CantLaunchException($"대기 중인 작업이 존재합니다.{Environment.NewLine}{Environment.NewLine}PE로 부팅하여 대기 중인 작업을 먼저 실행해 주세요.");
            } else if (settings.CurrentInstance is null) {
                InstallProcessor.CreateModel(true);
                MWindow = new InstallerMainWindow();
            } else {
                MWindow = new Views.MainWindow();
                MWindow.Closed += (_, _) => settings.SaveSettings();
            }
        } else {
            InstallProcessor.CreateModel(false);
            MWindow = new InstallerMainWindow();
        }

        MWindow.SetWindowSize(750, 500);
        MWindow.SetIsResizable(false);
        MWindow.SetIsMaximizable(false);
        MWindow.CenterOnScreen();
        MWindow.Activate();
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e) {
        var hWnd = MWindow != null ? WindowNative.GetWindowHandle(MWindow) : nint.Zero;
        var text = e.Exception is SimpleVhdException ? e.Exception.Message : e.Exception.ToString();

        _ = MessageBoxW(hWnd, text, null, 16);
        Exit();
    }

    private sealed class CantLaunchException(string message) : SimpleVhdException(message);
}
