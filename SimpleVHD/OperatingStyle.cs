namespace SimpleVHD;

/// <summary>
/// 운영 스타일
/// </summary>
public enum OperatingStyle {
    /// <summary>
    /// 단순 스타일
    /// </summary>
    Simple,
    /// <summary>
    /// 차등 스타일 (수동 초기화)
    /// </summary>
    DifferentialManual,
    /// <summary>
    /// 차등 스타일 (자동 초기화)
    /// </summary>
    DifferentialAuto
}