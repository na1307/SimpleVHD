using System.ComponentModel;

namespace SimpleVhd.Installer;

public partial class FormWorkingDialog : Form {
    protected FormWorkingDialog() => InitializeComponent();

    [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string LabelText {
        get => label1.Text;
        set => label1.Text = value;
    }
}
