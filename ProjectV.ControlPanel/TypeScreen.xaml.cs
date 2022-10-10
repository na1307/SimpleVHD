namespace ProjectV.ControlPanel;

/// <summary>
/// TypeScreen.xaml에 대한 상호 작용 논리
/// </summary>
public partial class TypeScreen {
    public TypeScreen(Screen backscreen) : base(backscreen) {
        InitializeComponent();
        ExpandableEntry.IsBack = PVConfig.Instance.VhdType == VhdType.Expandable;
        FixedEntry.IsBack = PVConfig.Instance.VhdType == VhdType.Fixed;
    }

    private void EntryControl_Start(object sender, EventArgs e) => DoProcess("형식", DoAction.DoConvertType, ((EntryControl)sender).Name switch {
        nameof(ExpandableEntry) => VhdType.Expandable,
        nameof(FixedEntry) => VhdType.Fixed,
        _ => throw new NotImplementedException()
    });

    private void EntryControl_Back(object sender, EventArgs e) => GoBack();
}