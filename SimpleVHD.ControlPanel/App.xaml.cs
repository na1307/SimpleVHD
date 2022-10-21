global using System.Diagnostics;
global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Media;
global using System.Windows.Media.Imaging;

namespace SimpleVHD.ControlPanel;

/// <summary>
/// App.xaml에 대한 상호 작용 논리
/// </summary>
public sealed partial class App {
    private static readonly Mutex mutex = new(true, "PVControl");

    internal static void SystemRestart() {
        using (Process shutdown = new() {
            StartInfo = {
                FileName = "shutdown.exe",
                Arguments = "/r /t 0",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden
            }
        }) {
            shutdown.Start();
        }

        Current.Shutdown();
    }

    private void Application_Startup(object sender, StartupEventArgs e) {
        if (!mutex.WaitOne(0)) {
            ErrMsg("이미 다른 인스턴스가 실행 중입니다.");
            Shutdown(1);
        }

        if (PVConfig.Instance.Action != DoAction.DoNothing) {
            ErrMsg("대기 중인 작업(재구축 등)이 있는 것 같습니다. 지금은 다른 작업을 진행할 수 없습니다.\r\n\r\n먼저 재부팅하여 대기중인 작업을 처리하시기 바랍니다.");
            Shutdown(1);
        }
    }

    private void Application_Exit(object sender, ExitEventArgs e) => PVConfig.Instance.SaveConfig();

    private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
        if (e.Exception is PVConfig.ConfigNotFoundException) {
            e.Handled = true;
            ErrMsg("설정 파일을 찾을 수 없습니다. SimpleVHD가 아예 설치되어 있지 않았을 수 있습니다.");
            Shutdown(1);
        } else if (e.Exception is PVConfig.InvalidConfigException) {
            e.Handled = true;
            ErrMsg(e.Exception.Message + (e.Exception.InnerException != null ? "\r\n\r\n" + e.Exception.InnerException.Message : null));
            Shutdown(1);
        } else {
            ErrMsg(e.Exception.ToString());
        }
    }
}