using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace SimpleVhd.ControlPanel;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    private void Application_Startup(object sender, StartupEventArgs e) {
        if (!Settings.IsSettingsJsonValid) {
            throw new SimpleVhdException("설정 파일이 올바르지 않습니다.");
        }
    }

    private void Application_Exit(object sender, ExitEventArgs e) {
        Settings.Instance.SaveSettings();
    }

    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
        if (e.Exception is SimpleVhdException) {
            MessageBox.Show(e.Exception.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        } else {
            MessageBox.Show(e.Exception.ToString(), "오류", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
