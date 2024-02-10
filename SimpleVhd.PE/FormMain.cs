namespace SimpleVhd.PE;

public partial class FormMain : Form {
    public FormMain() {
        InitializeComponent();
    }

    protected override CreateParams CreateParams {
        get {
            CreateParams myCp = base.CreateParams;
            myCp.ClassStyle |= 0x200;
            return myCp;
        }
    }

    private void button1_Click(object sender, EventArgs e) => Close();
}
