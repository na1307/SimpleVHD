using CommunityToolkit.Mvvm.ComponentModel;
using SimpleVhd.Installer.Views;

namespace SimpleVhd.Installer.ViewModels;

public sealed partial class InstallerMainWindowViewModel : ObservableObject {
    [ObservableProperty]
    private LinkedListNode<StepPage> currentPage = new LinkedList<StepPage>([new NamePage()]).First!;
}
