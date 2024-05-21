using Microsoft.UI.Xaml;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WinRT.Interop;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public sealed partial class App {
    private Window? m_window;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App() {
        try {
            Checker.Check();
        } catch (CheckException cex) {
            MessageBoxW(nint.Zero, cex.Message, null, 16);
            Process.GetCurrentProcess().Kill();
        }

        InitializeComponent();
        UnhandledException += App_UnhandledException;
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args) {
        m_window = File.Exists(Path.Combine(SVPath, SettingsFileName)) ? new MainWindow() : new Installer.MainWindow(Installer.InstallType.New);
        m_window.SetWindowSize(750, 500);
        m_window.SetIsResizable(false);
        m_window.SetIsMaximizable(false);
        m_window.CenterOnScreen();
        m_window.Activate();
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    private static extern int MessageBoxW(nint hWnd, string? text, string? caption, uint type);

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e) {
        var hWnd = m_window != null ? WindowNative.GetWindowHandle(m_window) : nint.Zero;
        var text = e.Exception is SimpleVhdException ? e.Exception.Message : e.Exception.ToString();

        MessageBoxW(hWnd, text, null, 16);
        Exit();
    }
}
