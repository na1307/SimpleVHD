namespace SimpleVHD.ControlPanel;

/// <summary>
/// OptionsScreen.xaml에 대한 상호 작용 논리
/// </summary>
public partial class OptionsScreen {
    public OptionsScreen() {
        InitializeComponent();
        SBackupBox.IsChecked = PVConfig.Instance.GetShutdown(DoAction.DoBackup);
        SRestoreBox.IsChecked = PVConfig.Instance.GetShutdown(DoAction.DoRestore);
        SRevertBox.IsChecked = PVConfig.Instance.GetShutdown(DoAction.DoRevert);
        SMergeBox.IsChecked = PVConfig.Instance.GetShutdown(DoAction.DoMerge);
        HidePEBox.IsChecked = !BcdEditRegexAll("/enum {bootmgr} /v", @"\{.+\}").Cast<System.Text.RegularExpressions.Match>().Any(guid => guid.Value == PVConfig.Instance.GetGuid(GuidType.PE));
    }

    private void ShutdownBox_Click(object sender, RoutedEventArgs e) => PVConfig.Instance.SetShutdown(((CheckBox)sender).Name switch {
        nameof(SBackupBox) => DoAction.DoBackup,
        nameof(SRestoreBox) => DoAction.DoRestore,
        nameof(SRevertBox) => DoAction.DoRevert,
        nameof(SMergeBox) => DoAction.DoMerge,
        _ => throw new NotImplementedException()
    }, ((CheckBox)sender).IsChecked!.Value);

    private void HidePEBox_Click(object sender, RoutedEventArgs e) => ProcessBcdEdit("/displayorder " + PVConfig.Instance.GetGuid(GuidType.PE) + (!HidePEBox.IsChecked!.Value ? " /addlast" : " /remove"));
}