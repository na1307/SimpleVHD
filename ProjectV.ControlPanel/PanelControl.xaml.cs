#nullable enable
namespace ProjectV.ControlPanel;

/// <summary>
/// PanelControl.xaml에 대한 상호 작용 논리
/// </summary>
public partial class PanelControl {
    private readonly ContentControl back;
    private readonly PanelAction pAction;

    public PanelControl(ContentControl backscreen, PanelAction action) {
        InitializeComponent();

        DoButton.Click += MainWindow.PlayClickSound;
        DoButton.Click += DoButton_Click;
        BackButton.Click += MainWindow.PlayClickSound;
        BackButton.Click += BackButton_Click;

        back = backscreen;
        pAction = action;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
        switch (pAction) {
            case PanelAction.DoBackup:
                PanelImage.Source = new BitmapImage(new(@"resources\dobackup.png", UriKind.Relative));
                TitleBlock.Text = BackupName;
                SubTitleBlock.Text = BackupDescription;
                ContentBlock1.Text = "현재 부모 VHD를 백업 폴더로 백업합니다.\r\n\r\n작업 시간 : 오래 걸림 (부모의 크기에 비례)";
                ShutdownBox.IsChecked = PVConfig.Instance[DoAction.DoBackup];
                break;

            case PanelAction.DoRestore:
                PanelImage.Source = new BitmapImage(new(@"resources\dorestore.png", UriKind.Relative));
                TitleBlock.Text = RestoreName;
                SubTitleBlock.Text = RestoreDescription;
                ContentBlock1.Text = "백업 폴더에 보관한 부모 VHD로 복원합니다.\r\n\r\n작업 시간 : 오래 걸림 (백업본의 크기에 비례)";
                ShutdownBox.IsChecked = PVConfig.Instance[DoAction.DoRestore];
                break;

            case PanelAction.DoRevert:
                PanelImage.Source = new BitmapImage(new(@"resources\dorevert.png", UriKind.Relative));
                TitleBlock.Text = RevertName;
                SubTitleBlock.Text = RevertDescription;
                ContentBlock1.Text = "변경분을 깨끗하게 초기화 합니다.\r\n\r\n작업 시간 : 아주 짧음";
                ShutdownBox.IsChecked = PVConfig.Instance[DoAction.DoRevert];
                break;

            case PanelAction.DoMerge:
                PanelImage.Source = new BitmapImage(new(@"resources\domerge.png", UriKind.Relative));
                TitleBlock.Text = MergeName;
                SubTitleBlock.Text = MergeDescription;
                ContentBlock1.Text = "변경분을 VHD에 병합합니다.\r\n\r\n작업 시간 : 오래 걸림 (변경분의 크기에 비례)";
                ShutdownBox.IsChecked = PVConfig.Instance[DoAction.DoMerge];
                break;

            case PanelAction.DoExpand:
                PanelImage.Source = new BitmapImage(new(@"resources\doexpand.png", UriKind.Relative));
                TitleBlock.Text = ExpandName;
                SubTitleBlock.Text = ExpandDescription;
                ContentBlock1.Text = "파티션은 자동으로 확장되지 않으며, 직접 확장하셔야 합니다.\r\n\r\n변경분이 초기화됩니다.\r\n\r\n작업 시간 : 짧음";
                ShutdownBox.Visibility = Visibility.Hidden;
                break;

            case PanelAction.DoShrink:
                PanelImage.Source = new BitmapImage(new(@"resources\doshrink.png", UriKind.Relative));
                TitleBlock.Text = ShrinkName;
                SubTitleBlock.Text = ShrinkDescription;
                ContentBlock1.Text = "변경분이 초기화됩니다.\r\n\r\n작업 시간 : 오래 걸림 (부모의 크기에 비례)";
                ShutdownBox.Visibility = Visibility.Hidden;
                break;

            case PanelAction.DoParentBoot:
                PanelImage.Source = new BitmapImage(new(@"resources\doparent.png", UriKind.Relative));
                TitleBlock.Text = ParentBootName;
                SubTitleBlock.Text = ParentBootDescription;
                ContentBlock1.Text = "변경분이 초기화됩니다.";
                ShutdownBox.Visibility = Visibility.Hidden;
                break;

            case PanelAction.DoUninstall:
                PanelImage.Source = new BitmapImage(new(@"resources\douninstall.png", UriKind.Relative));
                TitleBlock.Text = UninstallName;
                SubTitleBlock.Text = UninstallDescription;
                ContentBlock1.Text = "백업 파일은 삭제되지 않으며, " + DirName + " 폴더는 수동으로 지우셔야 합니다.";
                ShutdownBox.Visibility = Visibility.Hidden;
                break;

            case PanelAction.DoProcessorBoot:
                PanelImage.Source = new BitmapImage(new(@"resources\doprocessor.png", UriKind.Relative));
                TitleBlock.Text = ProcessorBootName;
                SubTitleBlock.Text = ProcessorBootDescription;
                ContentBlock1.Text = "작업기로 부팅 후 수동 작업을 수행하실 수 있습니다.";
                ShutdownBox.Visibility = Visibility.Hidden;
                break;

            default:
                throw new InvalidOperationException();
        }
    }

    private void DoButton_Click(object sender, RoutedEventArgs e) {
        if (MessageBox.Show("작업 실행 시 컴퓨터가 재부팅됩니다.\r\n\r\n정말 작업을 실행할까요?", "경고", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes) return;

        PVConfig.Instance.Action = (DoAction)pAction;
        ProcessBcdEdit("/bootsequence " + PVConfig.Instance[pAction != PanelAction.DoParentBoot && pAction != PanelAction.DoUninstall ? GuidType.Processor : GuidType.Parent]);
        App.SystemRestart();
    }

    private void BackButton_Click(object sender, RoutedEventArgs e) => MainWindow.ChangeContent(back);

    private void ShutdownBox_Click(object sender, RoutedEventArgs e) {
        switch (pAction) {
            case PanelAction.DoBackup:
                PVConfig.Instance[DoAction.DoBackup] = ShutdownBox.IsChecked.GetValueOrDefault();
                break;

            case PanelAction.DoRestore:
                PVConfig.Instance[DoAction.DoRestore] = ShutdownBox.IsChecked.GetValueOrDefault();
                break;

            case PanelAction.DoRevert:
                PVConfig.Instance[DoAction.DoRevert] = ShutdownBox.IsChecked.GetValueOrDefault();
                break;

            case PanelAction.DoMerge:
                PVConfig.Instance[DoAction.DoMerge] = ShutdownBox.IsChecked.GetValueOrDefault();
                break;

            default:
                throw new InvalidOperationException();
        }
    }
}