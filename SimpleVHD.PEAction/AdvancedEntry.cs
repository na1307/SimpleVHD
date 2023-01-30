using System.ComponentModel;

namespace SimpleVHD.PEAction;

[DefaultEvent("Start")]
public sealed partial class AdvancedEntry {
    public event EventHandler? Start;

    [Localizable(true)]
    [Bindable(true)]
    public string Title {
        get => label1.Text;
        set => label1.Text = value;
    }

    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [Bindable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public override string Text {
        get => label2.Text;
        set => label2.Text = value;
    }

    public AdvancedEntry() {
        InitializeComponent();
        button1.Click += (_, _) => Start?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnEnabledChanged(EventArgs e) {
        base.OnEnabledChanged(e);
        label2.Enabled = label1.Enabled = button1.Enabled = Enabled;
    }
}