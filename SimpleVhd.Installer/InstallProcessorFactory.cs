namespace SimpleVhd.Installer;

public static class InstallProcessorFactory {
    public static InstallProcessor Create(InstallType installType) => installType switch {
        InstallType.New => new NewInstallProcessor(),
        _ => throw new NotImplementedException(),
    };
}
