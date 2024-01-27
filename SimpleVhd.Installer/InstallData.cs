namespace SimpleVhd.Installer;

public abstract class InstallData {
    public string SVDrive { get; set; } = string.Empty;
    public string SVPath { get; set; } = string.Empty;
    public string SVDir { get; set; } = string.Empty;
    public string VhdDrive { get; set; } = string.Empty;
    public string VhdPath { get; set; } = string.Empty;
    public string VhdName { get; set; } = string.Empty;
    public VhdType Type { get; set; }
    public VhdFormat Format { get; set; }

    public abstract void CheckRequirements();
    public abstract void InstallProcess();
}
