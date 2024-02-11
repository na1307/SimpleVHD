namespace SimpleVhd.PE;

public partial class FormMain : FormNonClosed {
    private static readonly Lazy<FormMain> instance = new(() => new FormMain());

    private FormMain() => InitializeComponent();

    public static FormMain Instance => instance.Value;
    public static bool IsInitialized => instance.IsValueCreated;

    private static bool test() {
        using FormChooseInstance fci = new();

        return fci.ShowDialog() == DialogResult.OK;
    }

    private void button1_Click(object sender, EventArgs e) => Close();

    private void button2_Click(object sender, EventArgs e) {
        if (test()) {
            new FormWorking(OperationType.Restore).Show();
        }
    }

    private void button3_Click(object sender, EventArgs e) {
        throw new NotImplementedException();
    }

    private void button4_Click(object sender, EventArgs e) {
        throw new NotImplementedException();
    }

    private void button5_Click(object sender, EventArgs e) {
        throw new NotImplementedException();
    }
}
