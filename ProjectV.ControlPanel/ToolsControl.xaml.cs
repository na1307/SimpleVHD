#nullable enable
using static ProjectV.ControlPanel.MainWindow;
using static ProjectV.ControlPanel.PanelAction;

namespace ProjectV.ControlPanel;

/// <summary>
/// ToolsControl.xaml에 대한 상호 작용 논리
/// </summary>
public partial class ToolsControl {
    public ToolsControl() {
        InitializeComponent();

        foreach (var button in new[] { ParentButton, ProcessorButton, ExpandButton, ShrinkButton, TypeButton, FormatButton, StyleButton, UninstallButton }) {
            button.Click += PlayClickSound;
            button.Click += Button_Click;
        }
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
        var config = PVConfig.Instance;

        if (config.OperatingStyle is not (OperatingStyle.DifferentialManual or OperatingStyle.DifferentialAuto)) {
            ParentButton.IsEnabled = false;
        }

        if (!config.IsBackupExists()) {
            ShrinkButton.IsEnabled = false;
            TypeButton.IsEnabled = false;
            FormatButton.IsEnabled = false;
        }

        if (config.WinVer == WinVer.Seven) FormatButton.IsEnabled = false;
    }

    private void Button_Click(object sender, RoutedEventArgs e) => ChangeContent(this, ((Button)sender).Name switch {
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