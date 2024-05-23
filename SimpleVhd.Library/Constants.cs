namespace SimpleVhd;

public static class Constants {
    public const ulong BuildNumber = 1;
    public const string SVDirName = "SimpleVhd";
    public const string BackupDirName = "Backup";
    public const string Child1FileName = "Child1";
    public const string Child2FileName = "Child2";
    public const string SettingsFileName = "Settings.json";
    public const string SettingsSchemaUrl = "https://raw.githubusercontent.com/na1307/SimpleVhd/main/SimpleVhd.Library/Settings.schema.json";

    public static string SVPath { get; internal set; } = string.Empty;
}
