using System.Diagnostics;

namespace SimpleVhd.PE;

public partial class MainForm : NoCloseForm {
    private static readonly Lazy<MainForm> instance = new(() => new());

    private MainForm() => InitializeComponent();

    public static MainForm Instance => instance.Value;
    public static bool IsInitialized => instance.IsValueCreated;

    private static bool chooseInstance() {
        using ChooseInstanceDialog cid = new();

        return cid.ShowDialog() == DialogResult.OK;
    }

    private void button1_Click(object sender, EventArgs e) {
        if (chooseInstance()) {
            new Working(OperationType.Restore).Show();
        }
    }

    private void button2_Click(object sender, EventArgs e) {
        Process.Start("cmd.exe");
    }

    private void button3_Click(object sender, EventArgs e) {
        Close();
    }
}
