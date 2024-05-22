using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.Installer;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : INotifyPropertyChanged {
    private readonly InstallProcessor processor;
    private LinkedListNode<StepPage> cp;

    public MainWindow(InstallType installType) {
        InitializeComponent();
        processor = InstallProcessorFactory.Create(installType);
        cp = new LinkedList<StepPage>([new NamePage(processor), new VhdTypePage(processor)]).First!;

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

    private async void NextButton_Click(object sender, RoutedEventArgs e) {
        if (!processor.HasErrors) {
            if (CurrentPage.Next != null) {
                CurrentPage = CurrentPage.Next;
            } else {
                Content = new TextBlock() { Text = "설치 중...", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                await Task.Run(processor.InstallProcess);
                Content = new TextBlock() { Text = "설치 완료!", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                await Task.Delay(1500);
                Application.Current.Exit();
            }
        }
    }
}
