using System.Diagnostics;

namespace SimpleVhd.PE;

public partial class MainForm : NoCloseForm {
    public MainForm() => InitializeComponent();

    private void button1_Click(object sender, EventArgs e) {
        Process.Start("cmd.exe");
    }

    private void button2_Click(object sender, EventArgs e) {
        Close();
    }
}
