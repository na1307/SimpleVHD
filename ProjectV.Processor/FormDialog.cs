namespace ProjectV.Processor;

public partial class FormDialog {
    protected FormDialog() => InitializeComponent();

    protected sealed override void OnSizeChanged(EventArgs e) {
        base.OnSizeChanged(e);
        TableLayoutPanel1.Location = new(Size.Width - 190, Size.Height - 70);
    }

    protected virtual void OK_Button_Click(object sender, EventArgs e) {
        DialogResult = DialogResult.OK;
        Close();
    }

    protected virtual void Cancel_Button_Click(object sender, EventArgs e) {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}