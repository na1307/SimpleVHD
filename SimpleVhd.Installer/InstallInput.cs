using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleVhd.Installer;

public sealed class InstallInput : ObservableValidator {
    private string name = "여기에 이름을 입력해주세요";

    [Required(ErrorMessage = "이름은 비워둘 수 없습니다.")]
    public string Name {
        get => name;
        set => SetProperty(ref name, value, true);
    }

    public VhdType VhdType { get; set; }
}
