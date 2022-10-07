#nullable enable
namespace ProjectV.ControlPanel;

/// <summary>
/// TypeScreen.xaml에 대한 상호 작용 논리
/// </summary>
public partial class TypeScreen {
    private readonly ContentControl back;

    public TypeScreen(ContentControl backscreen) {
        InitializeComponent();

        foreach (var button in new[] { ExpandableButton, FixedButton }) {
            button.Click += MainWindow.PlayClickSound;
            button.Click += Button_Click;
        }

        back = backscreen;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
        switch (PVConfig.Instance.VhdType) {
            case VhdType.Fixed:
                FixedImage.Source = new BitmapImage(new(@"resources\back.png", UriKind.Relative));
                FixedBlock.Text = "뒤로";
                FixedBorder.BorderBrush = Brushes.Red;
                break;

            case VhdType.Expandable:
                ExpandableImage.Source = new BitmapImage(new(@"resources\back.png", UriKind.Relative));
                ExpandableBlock.Text = "뒤로";
                ExpandableBorder.BorderBrush = Brushes.Red;
                break;

            default:
                throw new NotImplementedException();
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
        var vt = PVConfig.Instance.VhdType;

        switch (((Button)sender).Name) {
            case nameof(ExpandableButton):
                if (vt != VhdType.Expandable) doProcess(VhdType.Expandable); else MainWindow.ChangeContent(back);
                break;

            case nameof(FixedButton):
                if (vt != VhdType.Fixed) doProcess(VhdType.Fixed); else MainWindow.ChangeContent(back);
                break;

            default:
                throw new NotImplementedException();
        }
    }

    private void doProcess(VhdType type) {
        if (MessageBox.Show("전환 작업 시 현재 차등 스타일을 사용 중인 경우 모든 변경분이 초기화됩니다.\r\n\r\n언제든지 다시 다른 형식으로 변경할 수 있습니다. 계속하시겠습니까?", "경고", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes) return;

        var config = PVConfig.Instance;

        config.Temp = type.ToString();
        config.Action = DoAction.DoConvertType;

        ProcessBcdEdit("/bootsequence " + config[GuidType.Processor]);
        App.SystemRestart();
    }
}