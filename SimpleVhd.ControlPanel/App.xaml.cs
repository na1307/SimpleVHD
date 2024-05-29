﻿using Microsoft.UI.Xaml;
using System.Diagnostics;
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
        try {
            Checker.CheckSvPath();
            Checker.CheckSystemVhd();
        } catch (CheckException cex) {
            _ = MessageBoxW(nint.Zero, cex.Message, null, 16);
            Process.GetCurrentProcess().Kill();
        }

        InitializeComponent();
        UnhandledException += App_UnhandledException;
    }

    public Window? MWindow { get; private set; }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override async void OnLaunched(LaunchActivatedEventArgs args) {
        if (File.Exists(Path.Combine(SVPath, SettingsFileName))) {
            await Checker.CheckSettingsJsonAsync();

            var settings = Settings.Instance;

            if (settings.OperationType == null) {
                if (settings.CurrentInstance is not null) {
                    MWindow = new MainWindow();
                    MWindow.Closed += (_, _) => settings.SaveSettings();
                } else {
                    MWindow = new Installer.InstallerMainWindow(Installer.InstallType.AddInstance);
                }
            } else {
                throw new CantLaunchException($"대기 중인 작업이 존재합니다.{Environment.NewLine}{Environment.NewLine}PE로 부팅하여 대기 중인 작업을 먼저 실행해 주세요.");
            }
        } else {
            MWindow = new Installer.InstallerMainWindow(Installer.InstallType.New);
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
