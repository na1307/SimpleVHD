using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using WinRT;

namespace SimpleVhd.ControlPanel;

internal static class Program {
    [STAThread]
    private static async Task Main() {
        ComWrappersSupport.InitializeComWrappers();

        if (!await decideRedirection()) {
            Application.Start(p => {
                SynchronizationContext.SetSynchronizationContext(new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread()));
                _ = new App();
            });
        }
    }

    private static async Task<bool> decideRedirection() {
        var isRedirect = false;
        var args = AppInstance.GetCurrent().GetActivatedEventArgs();
        var keyInstance = AppInstance.FindOrRegisterForKey("f50f8358-80b4-42b7-9b49-59f65bea3c9c");

        if (!keyInstance.IsCurrent) {
            isRedirect = true;
            await keyInstance.RedirectActivationToAsync(args);
        }

        return isRedirect;
    }
}
