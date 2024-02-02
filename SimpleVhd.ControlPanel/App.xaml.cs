using Microsoft.UI.Xaml;
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
    private Window? window;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App() {
        InitializeComponent();
        UnhandledException += App_UnhandledException;
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args) {
        //if (!Settings.IsSettingsJsonValid) {
        //    throw new SimpleVhdException("설정 파일이 올바르지 않습니다.");
        //}

        window = new MainWindow();
        window.Closed += Window_Closed;
        window.SetWindowSize(750, 500);
        window.SetIsResizable(false);
        window.SetIsMaximizable(false);
        window.CenterOnScreen();
        window.Activate();
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e) {
        _ = MessageBoxW(window is not null ? WindowNative.GetWindowHandle(window) : nint.Zero, e.Exception is SimpleVhdException ? e.Exception.Message : e.Exception.ToString(), "오류", 16);
        Exit();

        [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        static extern int MessageBoxW(nint hWnd, [MarshalAs(UnmanagedType.LPWStr)] string lpText, [MarshalAs(UnmanagedType.LPWStr)] string lpCaption, uint uType);
    }

    private void Window_Closed(object sender, WindowEventArgs args) {
        //Settings.Instance.SaveSettings();
    }
}
