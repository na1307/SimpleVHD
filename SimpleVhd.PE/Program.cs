namespace SimpleVhd.PE;

internal static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main() {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        try {
            BaseChecker.Check(true);
        } catch (CheckException ex) {
            ErrMsg(ex.Message);
            return;
        }

        Form? working = null;

        if (Settings.Instance.OperationType is not null) {
            if (Settings.Instance.InstanceToOperationOn is not null) {
                working = new FormWorking(Settings.Instance.OperationType.Value);
            } else {
                ErrMsg("OperationType이 설정되어 있지만 InstanceToOperationOn이 설정되어 있지 않습니다.");
                return;
            }
        }

        Application.ApplicationExit += Application_ApplicationExit;
        Application.Run(working ?? FormMain.Instance);
    }

    private static void Application_ApplicationExit(object? sender, EventArgs e) {
        Settings.Instance.SaveSettings();
        Application.ApplicationExit -= Application_ApplicationExit;
    }
}
