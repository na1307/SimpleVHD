using static ProjectV.ControlPanel.PanelAction;

namespace ProjectV.ControlPanel;

/// <summary>
/// ToolsScreen.xaml에 대한 상호 작용 논리
/// </summary>
public partial class ToolsScreen {
    public ToolsScreen() {
        InitializeComponent();

        foreach (var button in new[] { ParentButton, ProcessorButton, ExpandButton, ShrinkButton, TypeButton, FormatButton, StyleButton, UninstallButton }) {
            button.Click += MainWindow.PlayClickSound;
            button.Click += Button_Click;
        }

        if (!IsDifferentialStyle) {
            ParentButton.IsEnabled = false;
        }

        if (!BackupExists) {
            ShrinkButton.IsEnabled = false;
            TypeButton.IsEnabled = false;
            FormatButton.IsEnabled = false;
        }

        if (PVConfig.Instance.WindowsVersion == WindowsVersion.Seven) FormatButton.IsEnabled = false;
    }

    private void Button_Click(object sender, RoutedEventArgs e) => ((MainWindow)Application.Current.MainWindow).Screen = SubScreenFactory.Create(this, ((Button)sender).Name switch {
        nameof(ParentButton) => DoParentBoot,
        nameof(ProcessorButton) => DoProcessorBoot,
        nameof(ExpandButton) => DoExpand,
        nameof(ShrinkButton) => DoShrink,
        nameof(TypeButton) => DoConvertType,
        nameof(FormatButton) => DoConvertFormat,
        nameof(StyleButton) => DoSwitchStyle,
        nameof(UninstallButton) => DoUninstall,
        _ => throw new NotImplementedException()
    });
}