using CommunityToolkit.Mvvm.ComponentModel;
using SimpleVhd.Installer.Models;
using System.ComponentModel.DataAnnotations;

namespace SimpleVhd.Installer.ViewModels;

public sealed partial class NamePageViewModel : StepPageViewModel {
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(CanNext))]
    [Required(ErrorMessage = "이름은 비워둘 수 없습니다.")]
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
