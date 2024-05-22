using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleVhd.Installer;

public abstract class InstallProcessor : ObservableValidator {
    private string name = "여기에 이름을 입력해주세요";

    protected InstallProcessor() {
        SVDrive = SVPath.Left(2);
        var vp = GetSystemVhdPath();
        VhdDrive = Path.GetPathRoot(vp)!.TrimEnd('\\');
        VhdPath = Path.GetDirectoryName(vp[2..])!;
        VhdFileName = Path.GetFileNameWithoutExtension(vp);
        VhdFormat = Enum.Parse<VhdFormat>(Path.GetExtension(vp)[1..], true);
    }

    public string SVDrive { get; }
    public string VhdDrive { get; }
    public string VhdPath { get; }
    public string VhdFileName { get; }

    [Required(ErrorMessage = "이름은 비워둘 수 없습니다.")]
    public string Name {
        get => name;
        set => SetProperty(ref name, value, true);
    }

    public VhdType VhdType { get; set; }
    public VhdFormat VhdFormat { get; }

    public abstract void InstallProcess();
}
