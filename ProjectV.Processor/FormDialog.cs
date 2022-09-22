#nullable enable
namespace ProjectV.Processor;

public partial class FormDialog {
    protected FormDialog() => InitializeComponent();

    protected virtual void OK_Button_Click(object sender, EventArgs e) {
        DialogResult = DialogResult.OK;
        Close();
    }

    protected virtual void Cancel_Button_Click(object sender, EventArgs e) {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void FormDialog_SizeChanged(object sender, EventArgs e) => TableLayoutPanel1.Location = new(Size.Width - 190, Size.Height - 70);
}