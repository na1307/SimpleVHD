using CommunityToolkit.Mvvm.ComponentModel;
using SimpleVhd.Installer.Views;

namespace SimpleVhd.Installer.ViewModels;

public sealed partial class InstallerMainWindowViewModel : ObservableObject {
    [ObservableProperty]
    private LinkedListNode<StepPage> currentPage;

    public InstallerMainWindowViewModel(InstallType installType) {
        currentPage = new LinkedList<StepPage>([new NamePage()]).First!;
        InstallProcessor.CreateModel(installType);
    }
}
