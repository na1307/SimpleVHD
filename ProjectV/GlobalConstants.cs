namespace ProjectV;

/// <summary>
/// 전역 상수
/// </summary>
public static class GlobalConstants {
    /// <summary>
    /// 빌드 번호
    /// </summary>
    public const ulong BuildNumber = 7;

    /// <summary>
    /// Project V 디렉토리 이름
    /// </summary>
    public const string DirName = "ProjectV";

    /// <summary>
    /// Project V 백업 디렉토리 이름
    /// </summary>
    public const string BackupDirName = "PVBackup";

    /// <summary>
    /// Project V 설정 파일 이름
    /// </summary>
    public const string ConfigName = "Config.xml";

    /// <summary>
    /// XML 설정 파일 주석 내용
    /// </summary>
    public const string ConfigComment = "이 파일을 절대 편집하지 마세요!";

    /// <summary>
    /// 자식 1 파일 이름
    /// </summary>
    public static readonly string Child1Name = "Child1." + PVConfig.Instance.VhdFormat.ToString().ToLower();

    /// <summary>
    /// 자식 2 파일 이름
    /// </summary>
    public static readonly string Child2Name = "Child2." + PVConfig.Instance.VhdFormat.ToString().ToLower();

    /// <summary>
    /// 깨끗한 자식 파일 이름
    /// </summary>
    public static readonly string ChildCName = "Clean." + PVConfig.Instance.VhdFormat.ToString().ToLower();

    /// <summary>
    /// 현재 차등 스타일을 사용 중인지 여부
    /// </summary>
    public static readonly bool IsDifferentialStyle = PVConfig.Instance.OperatingStyle is OperatingStyle.DifferentialManual or OperatingStyle.DifferentialAuto;
}