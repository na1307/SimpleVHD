namespace ProjectV.ControlPanel;

/// <summary>
/// FormatScreen.xaml에 대한 상호 작용 논리
/// </summary>
public partial class FormatScreen {
    public FormatScreen(Screen backscreen) : base(backscreen) {
        InitializeComponent();
        VhdEntry.IsBack = PVConfig.Instance.VhdFormat == VhdFormat.Vhd;
        VhdxEntry.IsBack = PVConfig.Instance.VhdFormat == VhdFormat.Vhdx;
    }

    private void EntryControl_Start(object sender, EventArgs e) => DoProcess("포맷", DoAction.DoConvertFormat, ((EntryControl)sender).Name switch {
        nameof(VhdEntry) => VhdFormat.Vhd,
        nameof(VhdxEntry) => VhdFormat.Vhdx,
        _ => throw new NotImplementedException()
    });

    private void EntryControl_Back(object sender, EventArgs e) => GoBack();
}