namespace SimpleVHD.ControlPanel;

/// <summary>
/// ActionScreen.xaml에 대한 상호 작용 논리
/// </summary>
public partial class ActionScreen {
    private readonly PanelAction pAction;

    public ActionScreen(Screen backscreen, PanelAction action) : base(backscreen) {
        InitializeComponent();
        if (Properties.Settings.Default.UseExperimentalIcon) {
            SB.Source = (ImageSource)Application.Current.FindResource("StartIcon");
        }
        pAction = action;
        PanelImage.Source = new BitmapImage(new("resources\\" + pAction.ToString().ToLower() + ".png", UriKind.Relative));
        TitleBlock.Text = typeof(Constants).GetField(pAction.ToString().Substring(2) + "Name").GetValue(null).ToString();
        DescriptionBlock.Text = typeof(Constants).GetField(pAction.ToString().Substring(2) + "Description").GetValue(null).ToString();
        AdditionalBlock.Text = typeof(Constants).GetField(pAction.ToString().Substring(2) + "Additional").GetValue(null).ToString();
        if (ShutdownAction.Contains((DoAction)action)) ShutdownBox.IsChecked = PVConfig.Instance.GetShutdown((DoAction)pAction); else ShutdownBox.Visibility = Visibility.Hidden;
    }

    private void DoButton_Click(object sender, RoutedEventArgs e) {
        if (MessageBox.Show("작업 실행 시 컴퓨터가 재부팅됩니다.\r\n\r\n정말 작업을 실행할까요?", "경고", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes) return;

        PVConfig.Instance.Action = (DoAction)pAction;
        ProcessBcdEdit("/bootsequence " + PVConfig.Instance.GetGuid(pAction is not (PanelAction.DoParentBoot or PanelAction.DoUninstall) ? GuidType.PE : GuidType.Parent));
        App.SystemRestart();
    }

    private void BackButton_Click(object sender, RoutedEventArgs e) => GoBack();
    private void ShutdownBox_Click(object sender, RoutedEventArgs e) => PVConfig.Instance.SetShutdown((DoAction)pAction, ShutdownBox.IsChecked!.Value);
}