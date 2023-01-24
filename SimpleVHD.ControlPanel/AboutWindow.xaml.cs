using static Bluehill.AssemblyProperties;

namespace SimpleVHD.ControlPanel;

/// <summary>
/// AboutWindow.xaml에 대한 상호 작용 논리
/// </summary>
public sealed partial class AboutWindow {
    public AboutWindow() {
        InitializeComponent();
        Title = AssemblyTitle + " 정보";
        ProductBlock.Text = AssemblyProduct;
        VersionBlock.Text = $"버전 {AssemblyInformationalVersion} (빌드 {BuildNumber})";
        CopyrightBlock.Text = AssemblyCopyright;
    }

    private void Button_Click(object sender, RoutedEventArgs e) => Close();
}