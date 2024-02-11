namespace SimpleVhd.PE;

public class FormNonClosed : Form {
    protected override CreateParams CreateParams {
        get {
            CreateParams myCp = base.CreateParams;
            myCp.ClassStyle |= 512;
            return myCp;
        }
    }
}
