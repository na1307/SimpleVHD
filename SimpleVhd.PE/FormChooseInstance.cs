using Bluehill;

namespace SimpleVhd.PE;

public partial class FormChooseInstance : Dialog {
    public FormChooseInstance() {
        InitializeComponent();
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.DataSource = Settings.Instance.VhdInstances;
    }

    protected override void OK_Button_Click(object sender, EventArgs e) {
        var chosen = dataGridView1.SelectedRows[0].Index;
        var cv = Settings.Instance.VhdInstances[chosen];

        if (File.Exists(Path.Combine(BackupDir, $"{cv.ParentFile}.{cv.Format}"))) {
            Settings.Instance.InstanceToOperationOn = chosen;
            base.OK_Button_Click(sender, e);
        } else {
            ErrMsg("해당 인스턴스에 대한 백업 파일이 없습니다.");
            DialogResult = DialogResult.None;
        }
    }
}
