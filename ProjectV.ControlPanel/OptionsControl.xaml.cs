#nullable enable
namespace ProjectV.ControlPanel;

/// <summary>
/// OptionsControl.xaml에 대한 상호 작용 논리
/// </summary>
public partial class OptionsControl {
    public OptionsControl() => InitializeComponent();

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
        var config = PVConfig.Instance;

        SBackupBox.IsChecked = config[ShutdownType.Backup];
        SRestoreBox.IsChecked = config[ShutdownType.Restore];
        SRevertBox.IsChecked = config[ShutdownType.Revert];
        SMergeBox.IsChecked = config[ShutdownType.Merge];

        foreach (System.Text.RegularExpressions.Match guid in BcdEditRegexAll("/enum {bootmgr} /v", @"\{.+\}")) if (guid.Value == config[GuidType.Processor]) HideProcessorBox.IsChecked = false;
    }

    private void CheckBox_Click(object sender, RoutedEventArgs e) {
        var config = PVConfig.Instance;

        switch (((CheckBox)sender).Name) {
            case nameof(SBackupBox):
                config[ShutdownType.Backup] = SBackupBox.IsChecked.GetValueOrDefault();
                break;

            case nameof(SRestoreBox):
                config[ShutdownType.Restore] = SRestoreBox.IsChecked.GetValueOrDefault();
                break;

            case nameof(SRevertBox):
                config[ShutdownType.Revert] = SRevertBox.IsChecked.GetValueOrDefault();
                break;

            case nameof(SMergeBox):
                config[ShutdownType.Merge] = SMergeBox.IsChecked.GetValueOrDefault();
                break;

            case nameof(HideProcessorBox):
                ProcessBcdEdit("/displayorder " + config[GuidType.Processor] + (!HideProcessorBox.IsChecked.GetValueOrDefault() ? " /addlast" : " /remove"));
                break;

            default:
                throw new NotImplementedException();
        }
    }
}