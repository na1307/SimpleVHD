namespace ProjectV.ControlPanel;

/// <summary>
/// StyleScreen.xaml에 대한 상호 작용 논리
/// </summary>
public partial class StyleScreen {
    public StyleScreen(Screen backscreen) : base(backscreen) {
        InitializeComponent();
        SimpleEntry.IsBack = PVConfig.Instance.OperatingStyle == OperatingStyle.Simple;
        ManualEntry.IsBack = PVConfig.Instance.OperatingStyle == OperatingStyle.DifferentialManual;
        AutoEntry.IsBack = PVConfig.Instance.OperatingStyle == OperatingStyle.DifferentialAuto;
    }

    private void EntryControl_Start(object sender, EventArgs e) => DoProcess("스타일", DoAction.DoSwitchStyle, ((EntryControl)sender).Name switch {
        nameof(SimpleEntry) => OperatingStyle.Simple,
        nameof(ManualEntry) => OperatingStyle.DifferentialManual,
        nameof(AutoEntry) => OperatingStyle.DifferentialAuto,
        _ => throw new NotImplementedException()
    });

    private void EntryControl_Back(object sender, EventArgs e) => GoBack();
}