namespace ProjectV.ControlPanel;

/// <summary>
/// OptionsScreen.xaml에 대한 상호 작용 논리
/// </summary>
public partial class OptionsScreen {
    public OptionsScreen() {
        InitializeComponent();

        SBackupBox.IsChecked = PVConfig.Instance[DoAction.DoBackup];
        SRestoreBox.IsChecked = PVConfig.Instance[DoAction.DoRestore];
        SRevertBox.IsChecked = PVConfig.Instance[DoAction.DoRevert];
        SMergeBox.IsChecked = PVConfig.Instance[DoAction.DoMerge];

        foreach (var _ in BcdEditRegexAll("/enum {bootmgr} /v", @"\{.+\}").Cast<System.Text.RegularExpressions.Match>().Where(guid => guid.Value == PVConfig.Instance[GuidType.Processor])) {
            HideProcessorBox.IsChecked = false;
            break;
        }
    }

    private void ShutdownBox_Click(object sender, RoutedEventArgs e) => PVConfig.Instance[((CheckBox)sender).Name switch {
        nameof(SBackupBox) => DoAction.DoBackup,
        nameof(SRestoreBox) => DoAction.DoRestore,
        nameof(SRevertBox) => DoAction.DoRevert,
        nameof(SMergeBox) => DoAction.DoMerge,
        _ => throw new NotImplementedException()
    }] = ((CheckBox)sender).IsChecked!.Value;

    private void HideProcessorBox_Click(object sender, RoutedEventArgs e) => ProcessBcdEdit("/displayorder " + PVConfig.Instance[GuidType.Processor] + (!HideProcessorBox.IsChecked!.Value ? " /addlast" : " /remove"));
}