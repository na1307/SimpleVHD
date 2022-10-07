#nullable enable
namespace ProjectV.ControlPanel;

/// <summary>
/// FormatScreen.xaml에 대한 상호 작용 논리
/// </summary>
public partial class FormatScreen {
    private readonly ContentControl back;

    public FormatScreen(ContentControl backscreen) {
        InitializeComponent();

        foreach (var button in new[] { VhdButton, VhdxButton }) {
            button.Click += MainWindow.PlayClickSound;
            button.Click += Button_Click;
        }

        back = backscreen;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
        switch (PVConfig.Instance.VhdFormat) {
            case VhdFormat.Vhd:
                VhdImage.Source = new BitmapImage(new(@"resources\back.png", UriKind.Relative));
                VhdBlock.Text = "뒤로";
                VhdBorder.BorderBrush = Brushes.Red;
                break;

            case VhdFormat.Vhdx:
                VhdxImage.Source = new BitmapImage(new(@"resources\back.png", UriKind.Relative));
                VhdxBlock.Text = "뒤로";
                VhdxBorder.BorderBrush = Brushes.Red;
                break;

            default:
                throw new NotImplementedException();
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
        var vf = PVConfig.Instance.VhdFormat;

        switch (((Button)sender).Name) {
            case nameof(VhdButton):
                if (vf != VhdFormat.Vhd) doProcess(VhdFormat.Vhd); else MainWindow.ChangeContent(back);
                break;

            case nameof(VhdxButton):
                if (vf != VhdFormat.Vhdx) doProcess(VhdFormat.Vhdx); else MainWindow.ChangeContent(back);
                break;

            default:
                throw new NotImplementedException();
        }
    }

    private void doProcess(VhdFormat format) {
        if (MessageBox.Show("전환 작업 시 현재 차등 스타일을 사용 중인 경우 모든 변경분이 초기화됩니다.\r\n\r\n언제든지 다시 다른 포맷으로 변경할 수 있습니다. 계속하시겠습니까?", "경고", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes) return;

        var config = PVConfig.Instance;

        config.Temp = format.ToString();
        config.Action = DoAction.DoConvertFormat;

        ProcessBcdEdit("/bootsequence " + config[GuidType.Processor]);
        App.SystemRestart();
    }
}