#nullable enable
global using static ProjectV.ControlPanel.Constants;

namespace ProjectV.ControlPanel;

internal static class Constants {
    public const string BackupName = "백업";
    public const string BackupDescription = "부모 VHD를 백업하여 보관합니다.";
    public const string RestoreName = "복원";
    public const string RestoreDescription = "보관 중인 백업본으로 부모 VHD를 교체합니다.";
    public const string RevertName = "초기화";
    public const string RevertDescription = "변경분을 초기화합니다.";
    public const string MergeName = "병합";
    public const string MergeDescription = "변경분을 병합합니다.";
    public const string ParentBootName = "부모 VHD 부팅";
    public const string ParentBootDescription = "부모 VHD로 부팅합니다.";
    public const string ProcessorBootName = "작업기 부팅";
    public const string ProcessorBootDescription = "Project V 작업기로 부팅합니다.";
    public const string ExpandName = "VHD 확장";
    public const string ExpandDescription = "VHD의 최대 크기를 확장합니다.";
    public const string ShrinkName = "VHD 축소";
    public const string ShrinkDescription = "VHD의 최대 크기를 축소합니다.";
    public const string ConvertTypeName = "VHD 형식 변환";
    public const string ConvertTypeDescription = "VHD의 형식을 변환합니다.";
    public const string ConvertFormatName = "VHD 포맷 변환";
    public const string ConvertFormatDescription = "VHD의 포맷을 변환합니다.";
    public const string SwitchStyleName = "운영 스타일 전환";
    public const string SwitchStyleDescription = "Project V 운영 스타일을 전환합니다.";
    public const string UninstallName = "제거";
    public const string UninstallDescription = "Project V를 제거합니다.";
    public static readonly bool BackupExists = false;

    static Constants() {
        foreach (var drv in Directory.GetLogicalDrives()) {
            if (File.Exists(drv + BackupDirName + "\\" + PVConfig.Instance.VhdFile)) {
                BackupExists = true;
                break;
            }
        }
    }
}