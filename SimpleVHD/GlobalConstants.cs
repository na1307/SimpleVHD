namespace SimpleVHD;

/// <summary>
/// 전역 상수 모음
/// </summary>
public static class GlobalConstants {
    /// <summary>
    /// 빌드 번호
    /// </summary>
    public const ulong BuildNumber = 190;

    /// <summary>
    /// 핵심 디렉토리 이름
    /// </summary>
    public const string DirName = "SimpleVHD";

    /// <summary>
    /// 백업 디렉토리 이름
    /// </summary>
    public const string BackupDirName = $"{DirName}-{IncludedBackupDirName}";

    /// <summary>
    /// 포함된 백업 디렉토리 이름
    /// </summary>
    public const string IncludedBackupDirName = "Backup";

    /// <summary>
    /// 설정 파일 이름
    /// </summary>
    public const string ConfigName = "Config.xml";

    /// <summary>
    /// XML 설정 파일 주석 내용
    /// </summary>
    public const string ConfigComment = "이 파일을 절대 편집하지 마세요!";

    /// <summary>
    /// 설정 파일을 찾지 못했을 때 메시지
    /// </summary>
    public const string ConfigFileNotFoundMessage = "설정 파일을 찾을 수 없습니다.";

    /// <summary>
    /// 자식 1 파일 이름
    /// </summary>
    public const string Child1Name = "Child1.";

    /// <summary>
    /// 자식 2 파일 이름
    /// </summary>
    public const string Child2Name = "Child2.";

    /// <summary>
    /// 깨끗한 자식 파일 이름
    /// </summary>
    public const string ChildCName = "Clean.";
}