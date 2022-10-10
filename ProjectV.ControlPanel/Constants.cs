namespace ProjectV.ControlPanel;

internal static class Constants {
    public const string BackupName = "백업";
    public const string BackupDescription = "부모 VHD를 백업하여 보관합니다.";
    public const string BackupAdditional = "현재 부모 VHD를 백업 폴더로 백업합니다.\r\n\r\n작업 시간 : 오래 걸림 (부모의 크기에 비례)";
    public const string RestoreName = "복원";
    public const string RestoreDescription = "보관 중인 백업본으로 부모 VHD를 교체합니다.";
    public const string RestoreAdditional = "백업 폴더에 보관한 부모 VHD로 복원합니다.\r\n\r\n작업 시간 : 오래 걸림 (백업본의 크기에 비례)";
    public const string RevertName = "초기화";
    public const string RevertDescription = "변경분을 초기화합니다.";
    public const string RevertAdditional = "변경분을 깨끗하게 초기화 합니다.\r\n\r\n작업 시간 : 아주 짧음";
    public const string MergeName = "병합";
    public const string MergeDescription = "변경분을 병합합니다.";
    public const string MergeAdditional = "변경분을 VHD에 병합합니다.\r\n\r\n작업 시간 : 오래 걸림 (변경분의 크기에 비례)";
    public const string ParentBootName = "부모 VHD 부팅";
    public const string ParentBootDescription = "부모 VHD로 부팅합니다.";
    public const string ParentBootAdditional = "변경분이 초기화됩니다.";
    public const string ProcessorBootName = "작업기 부팅";
    public const string ProcessorBootDescription = "Project V 작업기로 부팅합니다.";
    public const string ProcessorBootAdditional = "작업기로 부팅 후 수동 작업을 수행하실 수 있습니다.";
    public const string ExpandName = "VHD 확장";
    public const string ExpandDescription = "VHD의 최대 크기를 확장합니다.";
    public const string ExpandAdditional = "파티션은 자동으로 확장되지 않으며, 직접 확장하셔야 합니다.\r\n\r\n변경분이 초기화됩니다.\r\n\r\n작업 시간 : 짧음";
    public const string ShrinkName = "VHD 축소";
    public const string ShrinkDescription = "VHD의 최대 크기를 축소합니다.";
    public const string ShrinkAdditional = "변경분이 초기화됩니다.\r\n\r\n작업 시간 : 오래 걸림 (부모의 크기에 비례)";
    public const string ConvertTypeName = "VHD 형식 변환";
    public const string ConvertTypeDescription = "VHD의 형식을 변환합니다.";
    public const string ConvertFormatName = "VHD 포맷 변환";
    public const string ConvertFormatDescription = "VHD의 포맷을 변환합니다.";
    public const string SwitchStyleName = "운영 스타일 전환";
    public const string SwitchStyleDescription = "Project V 운영 스타일을 전환합니다.";
    public const string UninstallName = "제거";
    public const string UninstallDescription = "Project V를 제거합니다.";
    public const string UninstallAdditional = "백업 파일은 삭제되지 않으며, " + DirName + " 폴더는 수동으로 지우셔야 합니다.";
}