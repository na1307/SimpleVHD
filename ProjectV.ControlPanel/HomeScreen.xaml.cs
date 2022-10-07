#nullable enable
using static ProjectV.ControlPanel.MainWindow;
using static ProjectV.ControlPanel.PanelAction;

namespace ProjectV.ControlPanel;

/// <summary>
/// HomeScreen.xaml에 대한 상호 작용 논리
/// </summary>
public partial class HomeScreen {
    public HomeScreen() {
        InitializeComponent();

        foreach (var button in new[] { RevertButton, MergeButton, BackupButton, RestoreButton }) button.Click += PlayClickSound;
    }

    private void UserControl_Initialized(object sender, EventArgs e) {
        if (!BackupExists) RestoreButton.IsEnabled = false;

        switch (PVConfig.Instance.OperatingStyle) {
            case OperatingStyle.Simple:
                RevertButton.Visibility = Visibility.Collapsed;
                MergeButton.Visibility = Visibility.Collapsed;
                TheUniformGrid.Rows = 1;
                break;

            case OperatingStyle.DifferentialManual:
                break;

            case OperatingStyle.DifferentialAuto:
                RevertButton.IsEnabled = false;
                break;

            default:
                throw new PVConfig.InvalidConfigException("OperatingStyle이 잘못되었습니다.");
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e) => ChangeContent(this, ((Button)sender).Name switch {
        nameof(RevertButton) => DoRevert,
        nameof(MergeButton) => DoMerge,
        nameof(BackupButton) => DoBackup,
        nameof(RestoreButton) => DoRestore,
        _ => throw new NotImplementedException()
    });
}