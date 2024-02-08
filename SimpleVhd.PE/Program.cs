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

        Application.Run(new FormMain());
    }
}