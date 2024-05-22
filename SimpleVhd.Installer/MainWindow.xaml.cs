using Microsoft.UI.Xaml;
using System.ComponentModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.Installer;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : INotifyPropertyChanged {
    private readonly InstallType installType;
    private readonly InstallInput input;
    private LinkedListNode<StepPage> cp;

    public MainWindow(InstallType installType) {
        InitializeComponent();
        this.installType = installType;
        input = new();
        cp = new LinkedList<StepPage>([new NamePage(input), new VhdTypePage(input)]).First!;

        PropertyChanged += (_, e) => {
            if (e.PropertyName == nameof(CurrentPage)) {
                NextButton.Content = CurrentPage.Next != null ? "다음" : "완료";
                BackButton.IsEnabled = CurrentPage.Previous != null;
            }
        };
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private LinkedListNode<StepPage> CurrentPage {
        get => cp;
        set {
            cp = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));
        }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e) {
        if (CurrentPage.Previous != null) {
            CurrentPage = CurrentPage.Previous;
        } else {
            throw new InvalidOperationException();
        }
    }

    private void NextButton_Click(object sender, RoutedEventArgs e) {
        if (!input.HasErrors) {
            if (CurrentPage.Next != null) {
                CurrentPage = CurrentPage.Next;
            } else {
                // Do Something
            }
        }
    }
}
