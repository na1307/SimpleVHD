namespace ProjectV;

/// <summary>
/// 확장 메서드 모음
/// </summary>
public static class Extensions {
    /// <summary>
    /// <paramref name="source"/> 문자열에서 왼쪽으로 <paramref name="length"/>만큼의 문자열을 반환
    /// </summary>
    /// <param name="source">원본 문자열</param>
    /// <param name="length">자를 길이</param>
    /// <returns><paramref name="source"/>에서 <paramref name="length"/>만큼 자른 문자열</returns>
    public static string Left(this string source, int length) => source.Substring(0, length);

    /// <summary>
    /// <paramref name="source"/> 문자열에서 오른쪽으로 <paramref name="length"/>만큼의 문자열을 반환
    /// </summary>
    /// <param name="source">원본 문자열</param>
    /// <param name="length">자를 길이</param>
    /// <returns><paramref name="source"/>에서 <paramref name="length"/>만큼 자른 문자열</returns>
    public static string Right(this string source, int length) => source.Substring(source.Length - length);

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