using CommunityToolkit.Mvvm.ComponentModel;

namespace SimpleVhd.Installer.ViewModels;

public abstract class StepPageViewModel : ObservableObject {
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract bool CanNext { get; }
    public abstract bool CanBack { get; }
}
