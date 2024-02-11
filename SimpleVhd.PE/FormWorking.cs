namespace SimpleVhd.PE;

public partial class FormWorking : FormNonClosed {
    private readonly Operation operation;

    public FormWorking(OperationType operationType) {
        InitializeComponent();
        operation = OperationFactory.Create(operationType);
        label1.Text = operation.OperationName + label1.Text;
    }

    protected override async void OnShown(EventArgs e) {
        base.OnShown(e);

        if (FormMain.IsInitialized) {
            FormMain.Instance.Hide();
        }

        await operation.WorkAsync();
        Close();
    }

    protected override void OnClosed(EventArgs e) {
        base.OnClosed(e);

        if (FormMain.IsInitialized) {
            FormMain.Instance.Close();
        }
    }
}
