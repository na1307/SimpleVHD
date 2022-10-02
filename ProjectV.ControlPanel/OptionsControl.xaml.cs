#nullable enable
namespace ProjectV.ControlPanel;

/// <summary>
/// OptionsControl.xaml에 대한 상호 작용 논리
/// </summary>
public partial class OptionsControl {
    public OptionsControl() => InitializeComponent();

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
        var config = PVConfig.Instance;

        SBackupBox.IsChecked = config[DoAction.DoBackup];
        SRestoreBox.IsChecked = config[DoAction.DoRestore];
        SRevertBox.IsChecked = config[DoAction.DoRevert];
        SMergeBox.IsChecked = config[DoAction.DoMerge];

        foreach (System.Text.RegularExpressions.Match guid in BcdEditRegexAll("/enum {bootmgr} /v", @"\{.+\}")) if (guid.Value == config[GuidType.Processor]) HideProcessorBox.IsChecked = false;
    }

    private void CheckBox_Click(object sender, RoutedEventArgs e) {
        var config = PVConfig.Instance;

        switch (((CheckBox)sender).Name) {
            case nameof(SBackupBox):
                config[DoAction.DoBackup] = SBackupBox.IsChecked.GetValueOrDefault();
                break;

            case nameof(SRestoreBox):
                config[DoAction.DoRestore] = SRestoreBox.IsChecked.GetValueOrDefault();
                break;

            case nameof(SRevertBox):
                config[DoAction.DoRevert] = SRevertBox.IsChecked.GetValueOrDefault();
                break;

            case nameof(SMergeBox):
                config[DoAction.DoMerge] = SMergeBox.IsChecked.GetValueOrDefault();
                break;

            case nameof(HideProcessorBox):
                ProcessBcdEdit("/displayorder " + config[GuidType.Processor] + (!HideProcessorBox.IsChecked.GetValueOrDefault() ? " /addlast" : " /remove"));
                break;

            default:
                throw new NotImplementedException();
        }
    }
}