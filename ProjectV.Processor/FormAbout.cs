#nullable enable
using static ProjectV.AssemblyProperties;

namespace ProjectV.Processor;

public partial class FormAbout {
    public FormAbout() {
        InitializeComponent();
        Text = AssemblyTitle + " 정보";
        pictureBox1.Image = Application.OpenForms["FormMain"].Icon.ToBitmap();
        label1.Text = AssemblyProduct;
        label2.Text = $"버전 {AssemblyInformationalVersion} (빌드 {BuildNumber})";
        label3.Text = AssemblyCopyright;
        label4.Text = "현재 설정된 VHD : " + VHDDir + PVConfig.Instance.VhdFile;
        label5.Text = "현재 설정된 백업 : " + BackupDir + PVConfig.Instance.VhdFile;
    }

    private void okButton_Click(object sender, EventArgs e) => Close();
}