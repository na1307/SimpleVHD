namespace SimpleVhd.Installer;

public abstract class InstallProcessor {
    protected InstallProcessor() {
        SVDrive = SVPath.Left(2);
        var vp = GetSystemVhdPath();
        VhdDrive = Path.GetPathRoot(vp)!.TrimEnd('\\');
        VhdPath = Path.GetDirectoryName(vp[2..])!;

        if (VhdPath.Length > 1) {
            VhdPath += "\\";
        }

        VhdFileName = Path.GetFileNameWithoutExtension(vp);
        VhdFormat = Enum.Parse<VhdFormat>(Path.GetExtension(vp)[1..], true);
    }

    public static InstallProcessor? Model { get; private set; }
    public string SVDrive { get; }
    public string VhdDrive { get; }
    public string VhdPath { get; }
    public string VhdFileName { get; }
    public string Name { get; set; } = "여기에 이름을 입력해주세요";
    public VhdType VhdType { get; set; }
    public VhdFormat VhdFormat { get; }

    public static void CreateModel(InstallType installType) => Model = installType switch {
        InstallType.New => new NewInstallProcessor(),
        InstallType.AddInstance => new AddInstanceInstallProcessor(),
        _ => throw new NotImplementedException(),
    };

    public abstract void InstallProcess();
}
