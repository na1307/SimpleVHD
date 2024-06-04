using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleVhd.Installer.Views;

namespace SimpleVhd.Installer.ViewModels;

public sealed partial class InstallerMainWindowViewModel : ObservableObject {
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    [NotifyPropertyChangedFor(nameof(Description))]
    [NotifyCanExecuteChangedFor(nameof(GoNextCommand))]
    [NotifyCanExecuteChangedFor(nameof(GoBackCommand))]
    private LinkedListNode<StepPage> currentPage = new LinkedList<StepPage>([new NamePage(), new VhdTypePage(), new InstallingPage()]).First!;

    public string Title => CurrentPage.Value.ViewModel.Title;
    public string Description => CurrentPage.Value.ViewModel.Description;

    private bool CanNext => CurrentPage.Value.ViewModel.CanNext;
    private bool CanBack => CurrentPage.Value.ViewModel.CanBack;

    [RelayCommand(CanExecute = nameof(CanNext))]
    private void GoNext() => CurrentPage = CurrentPage.Next!;

    [RelayCommand(CanExecute = nameof(CanBack))]
    private void GoBack() => CurrentPage = CurrentPage.Previous!;
}
