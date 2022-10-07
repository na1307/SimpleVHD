#nullable enable
namespace ProjectV.ControlPanel;

/// <summary>
/// StyleScreen.xaml에 대한 상호 작용 논리
/// </summary>
public partial class StyleScreen {
    private readonly ContentControl back;

    public StyleScreen(ContentControl backscreen) {
        InitializeComponent();

        foreach (var button in new[] { SimpleButton, DifferentialManualButton, DifferentialAutoButton }) {
            button.Click += MainWindow.PlayClickSound;
            button.Click += Button_Click;
        }

        back = backscreen;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
        switch (PVConfig.Instance.OperatingStyle) {
            case OperatingStyle.Simple:
                SimpleImage.Source = new BitmapImage(new(@"resources\back.png", UriKind.Relative));
                SimpleBlock.Text = "뒤로";
                SimpleBorder.BorderBrush = Brushes.Red;
                break;

            case OperatingStyle.DifferentialManual:
                DifferentialManualImage.Source = new BitmapImage(new(@"resources\back.png", UriKind.Relative));
                DifferentialManualBlock.Text = "뒤로";
                DifferentialManualBorder.BorderBrush = Brushes.Red;
                break;

            case OperatingStyle.DifferentialAuto:
                DifferentialAutoImage.Source = new BitmapImage(new(@"resources\back.png", UriKind.Relative));
                DifferentialAutoBlock.Text = "뒤로";
                DifferentialAutoBorder.BorderBrush = Brushes.Red;
                break;

            default:
                throw new NotImplementedException();
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
        var os = PVConfig.Instance.OperatingStyle;

        switch (((Button)sender).Name) {
            case nameof(SimpleButton):
                if (os != OperatingStyle.Simple) doProcess(OperatingStyle.Simple); else MainWindow.ChangeContent(back);
                break;

            case nameof(DifferentialManualButton):
                if (os != OperatingStyle.DifferentialManual) doProcess(OperatingStyle.DifferentialManual); else MainWindow.ChangeContent(back);
                break;

            case nameof(DifferentialAutoButton):
                if (os != OperatingStyle.DifferentialAuto) doProcess(OperatingStyle.DifferentialAuto); else MainWindow.ChangeContent(back);
                break;

            default:
                throw new NotImplementedException();
        }
    }

    private void doProcess(OperatingStyle style) {
        if (MessageBox.Show("전환 작업 시 현재 차등 스타일을 사용 중인 경우 모든 변경분이 초기화됩니다.\r\n\r\n언제든지 다시 다른 스타일로 변경할 수 있습니다. 계속하시겠습니까?", "경고", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes) return;

        var config = PVConfig.Instance;

        config.Temp = style.ToString();
        config.Action = DoAction.DoSwitchStyle;

        ProcessBcdEdit("/bootsequence " + config[GuidType.Processor]);
        App.SystemRestart();
    }
}