namespace SimpleVhd.PE;

public sealed partial class ChooseInstanceDialog : Dialog {
    public ChooseInstanceDialog() {
        InitializeComponent();
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.DataSource = Settings.Instance.Instances;
    }

    protected override void OK_Button_Click(object sender, EventArgs e) {
        var chosen = dataGridView1.SelectedRows[0].Index;
        var cv = Settings.Instance.Instances[chosen];

        if (File.Exists(Path.Combine(SVPath, BackupDirName, $"{cv.FileName}.{cv.Format}"))) {
            Settings.Instance.OperationTarget = chosen;
            base.OK_Button_Click(sender, e);
        } else {
            ErrMsg("해당 인스턴스에 대한 백업 파일이 없습니다.");
            DialogResult = DialogResult.None;
        }
    }
}
