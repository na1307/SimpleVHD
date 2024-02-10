namespace SimpleVhd.Startup;

internal static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3626:Jump statements should not be redundant", Justification = "<º¸·ù Áß>")]
    private static void Main() {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        try {
            //BaseChecker.Check(true);
        } catch (CheckException ex) {
            ErrMsg(ex.Message);
            return;
        }
    }
}
