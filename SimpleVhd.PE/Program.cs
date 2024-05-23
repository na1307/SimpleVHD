namespace SimpleVhd.PE;

internal static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static async Task Main() {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        try {
            Checker.Check();
            await Checker.CheckSettingsJsonAsync();
        } catch (CheckException cex) {
            ErrMsg(cex.Message);
            return;
        }

        var settings = Settings.Instance;
        Form? working = null;

        if (settings.OperationType is not null) {
            if (settings.InstanceToOperationOn is not null) {
                working = new Working(settings.OperationType.Value);
            } else {
                ErrMsg("OperationType�� �����Ǿ� ������ InstanceToOperationOn�� �����Ǿ� ���� �ʽ��ϴ�.");
                return;
            }
        }

        Application.ApplicationExit += Application_ApplicationExit;
        Application.Run(working ?? new MainForm());
    }

    private static void Application_ApplicationExit(object? sender, EventArgs e) => Settings.Instance.SaveSettings();
}
