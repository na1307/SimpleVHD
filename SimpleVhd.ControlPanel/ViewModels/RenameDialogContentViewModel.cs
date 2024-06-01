using CommunityToolkit.Mvvm.ComponentModel;

namespace SimpleVhd.ControlPanel.ViewModels;

public sealed partial class RenameDialogContentViewModel : ObservableObject {
    [ObservableProperty]
    private string name = Settings.Instance.CurrentInstance!.Name;
}
