namespace SimpleVHD;

/// <summary>
/// 확장 메서드 모음
/// </summary>
public static class Extensions {
    /// <summary>
    /// 드라이브가 고정 드라이브인지 여부를 반환
    /// </summary>
    /// <param name="drive">드라이브</param>
    /// <returns>드라이브가 고정 드라이브인지 여부</returns>
    public static bool CheckFixed(this DriveInfo drive) => drive.DriveType == DriveType.Fixed;

    /// <summary>
    /// 드라이브의 문자(예: C:)를 반환
    /// </summary>
    /// <param name="drive">드라이브</param>
    /// <returns>드라이브 문자</returns>
    public static string GetLetter(this DriveInfo drive) => drive.Name.Left(2);
}