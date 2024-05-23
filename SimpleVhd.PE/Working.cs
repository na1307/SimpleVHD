using SimpleVhd.PE.Operations;

namespace SimpleVhd.PE;

public partial class Working : NoCloseForm {
    private readonly Operation operation;

    public Working(OperationType operationType) {
        InitializeComponent();
        operation = OperationFactory.Create(operationType);
        label1.Text = operation.OperationName + label1.Text;
    }

    protected override async void OnShown(EventArgs e) {
        base.OnShown(e);

        if (MainForm.IsInitialized) {
            MainForm.Instance.Hide();
        }

        await operation.WorkAsync();
        Close();
    }

    protected override void OnClosed(EventArgs e) {
        base.OnClosed(e);

        if (MainForm.IsInitialized) {
            MainForm.Instance.Close();
        }
    }
}
