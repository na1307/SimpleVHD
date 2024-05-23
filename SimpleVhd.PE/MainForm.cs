using System.Diagnostics;

namespace SimpleVhd.PE;

public partial class MainForm : NoCloseForm {
    private static readonly Lazy<MainForm> instance = new(() => new());

    private MainForm() => InitializeComponent();

    public static MainForm Instance => instance.Value;
    public static bool IsInitialized => instance.IsValueCreated;

    private void button1_Click(object sender, EventArgs e) {
        Process.Start("cmd.exe");
    }

    private void button2_Click(object sender, EventArgs e) {
        Close();
    }
}
