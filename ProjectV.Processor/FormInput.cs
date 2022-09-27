#nullable enable
namespace ProjectV.Processor;

public partial class FormInput {
    public string Input => textBox1.Text;

    public FormInput(string title, string content) {
        InitializeComponent();
        Text = title;
        label1.Text = content;
    }

    protected override void OK_Button_Click(object sender, EventArgs e) {
        try {
            ulong.Parse(textBox1.Text);
        } catch (FormatException) {
            MessageBox.Show("숫자만 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        base.OK_Button_Click(sender, e);
    }
}