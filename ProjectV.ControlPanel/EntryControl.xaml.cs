namespace ProjectV.ControlPanel;

/// <summary>
/// EntryControl.xaml에 대한 상호 작용 논리
/// </summary>
public sealed partial class EntryControl {
    private bool _IsBack;

    public event EventHandler? Start;
    public event EventHandler? Back;

    public string? Title { get; init; }
    public string? Description { get; init; }

    public bool IsBack {
        get => _IsBack;
        set {
            _IsBack = value;
            startButton.Visibility = !value ? Visibility.Visible : Visibility.Collapsed;
            backButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            entryBorder.BorderBrush = (Brush)new BrushConverter().ConvertFromString(!value ? "LightGray" : "Red");
        }
    }

    public EntryControl() {
        InitializeComponent();
        DataContext = this;
        startButton.Click += MainWindow.PlayClickSound;
        startButton.Click += (s, e) => Start?.Invoke(this, EventArgs.Empty);
        backButton.Click += MainWindow.PlayClickSound;
        backButton.Click += (s, e) => Back?.Invoke(this, EventArgs.Empty);
    }
}