using CommunityToolkit.Mvvm.ComponentModel;

namespace SimpleVhd.Installer.ViewModels;

public sealed partial class NamePageViewModel : StepPageViewModel {
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanNext))]
    private string name = InstallProcessor.Model!.Name;

    public override string Title => "인스턴스 이름";
    public override string Description => "이 VHD 인스턴스의 이름(별명)을 입력해 주세요.";

    public override bool CanNext => !string.IsNullOrWhiteSpace(Name);
    public override bool CanBack => false;

    partial void OnNameChanged(string? oldValue, string newValue) {
        if (newValue != oldValue) {
            InstallProcessor.Model!.Name = newValue;
        }
    }
}
