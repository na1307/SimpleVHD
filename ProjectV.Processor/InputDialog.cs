#nullable enable
namespace ProjectV.Processor;

public partial class InputDialog {
    public string Input => textBox1.Text;

    public InputDialog(string title, string content) {
        InitializeComponent();

        Text = title;
        label1.Text = content;
    }

    private void OK_Button_Click(object sender, EventArgs e) {
        try {
            ulong.Parse(textBox1.Text);
        } catch (FormatException) {
            MessageBox.Show("숫자만 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void Cancel_Button_Click(object sender, EventArgs e) {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}