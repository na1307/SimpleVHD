#nullable enable
namespace ProjectV.ControlPanel;

/// <summary>
/// MainWindow.xaml에 대한 상호 작용 논리
/// </summary>
public partial class MainWindow {
    internal static void PlayClickSound(object sender, RoutedEventArgs e) => new System.Media.SoundPlayer(Properties.Resources.Interaction).Play();

    internal static void ChangeContent(ContentControl parent, PanelAction action) {
        switch (action) {
            case PanelAction.DoConvertType:
                ChangeContent(new TypeControl(parent));
                break;

            case PanelAction.DoConvertFormat:
                ChangeContent(new FormatControl(parent));
                break;

            case PanelAction.DoSwitchStyle:
                ChangeContent(new StyleControl(parent));
                break;

            default:
                ChangeContent(new PanelControl(parent, action));
                break;
        }
    }

    internal static void ChangeContent(ContentControl newcontent) => ((MainWindow)Application.Current.MainWindow).ContentArea.Content = newcontent;

    public MainWindow() {
        InitializeComponent();

        foreach (var button in new[] { HomeButton, ToolsButton, OptionButton, HelpButton, AboutButton, ExitButton }) {
            button.Click += PlayClickSound;
            button.Click += Button_Click;
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) {
        ContentArea.Content = new HomeControl();
        AboutButton.ToolTip = AssemblyProperties.AssemblyTitle + (string)AboutButton.ToolTip;
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
        switch (((Button)sender).Name) {
            case nameof(HomeButton):
                ContentArea.Content = new HomeControl();
                break;

            case nameof(ToolsButton):
                ContentArea.Content = new ToolsControl();
                break;

            case nameof(OptionButton):
                ContentArea.Content = new OptionsControl();
                break;

            case nameof(HelpButton):
                Process.Start("https://na1307.github.io/ProjectV/");
                break;

            case nameof(AboutButton):
                new AboutWindow() { Owner = this }.ShowDialog();
                break;

            case nameof(ExitButton):
                Application.Current.Shutdown();
                break;

            default:
                throw new InvalidOperationException();
        }
    }
}