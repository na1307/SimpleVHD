namespace ProjectV;

/// <summary>
/// bcd GUID 타입
/// </summary>
public enum GuidType {
    /// <summary>
    /// 부모 VHD의 GUID
    /// </summary>
    Parent,
    /// <summary>
    /// 자식 1 VHD의 GUID
    /// </summary>
    Child1,
    /// <summary>
    /// 자식 2 VHD의 GUID
    /// </summary>
    Child2,
    /// <summary>
    /// PE에서 사용할 boot.sdi 설정 GUID
    /// </summary>
    Ramdisk,
    /// <summary>
    /// PE의 GUID
    /// </summary>
    PE
}