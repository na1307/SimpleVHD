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

    /// <summary>
    /// 각 요소에 대해 지정한 작업을 수행함
    /// </summary>
    /// <typeparam name="T">요소의 형식</typeparam>
    /// <param name="enumerable"><see cref="IEnumerable{T}"/></param>
    /// <param name="action">수행할 작업</param>
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
        if (enumerable is T[] array) {
            Array.ForEach(array, action);
        } else if (enumerable is List<T> list) {
            list.ForEach(action);
        } else {
            new List<T>(enumerable).ForEach(action);
        }
    }
}