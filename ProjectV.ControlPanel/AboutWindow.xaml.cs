using static ProjectV.AssemblyProperties;

namespace ProjectV.ControlPanel;

/// <summary>
/// AboutWindow.xaml에 대한 상호 작용 논리
/// </summary>
public partial class AboutWindow {
    public AboutWindow() {
        InitializeComponent();
        Title = AssemblyTitle + " 정보";
        ProductBlock.Text = AssemblyProduct;
        VersionBlock.Text = $"버전 {AssemblyInformationalVersion} (빌드 {BuildNumber})";
        CopyrightBlock.Text = AssemblyCopyright;
    }

    private void Button_Click(object sender, RoutedEventArgs e) => Close();
}