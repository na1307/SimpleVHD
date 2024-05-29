namespace SimpleVhd.PE;

internal static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static async Task Main() {
        try {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            try {
                Checker.CheckSvPath();
                await Checker.CheckSettingsJsonAsync();
            } catch (CheckException cex) {
                ErrMsg(cex.Message);
                return;
            }

            var settings = Settings.Instance;
            Form? working = null;

            if (settings.OperationType is not null) {
                if (settings.OperationTarget is not null) {
                    working = new Working(settings.OperationType.Value);
                } else {
                    ErrMsg("OperationType이 설정되어 있지만 OperationTarget이 설정되어 있지 않습니다.");
                    return;
                }
            }

            Application.ApplicationExit += Application_ApplicationExit;
            Application.Run(working ?? MainForm.Instance);
        } catch (Exception e) {
            ErrMsg(e.ToString());
            Application.Exit();
        }
    }

    private static void Application_ApplicationExit(object? sender, EventArgs e) => Settings.Instance.SaveSettings();
}
