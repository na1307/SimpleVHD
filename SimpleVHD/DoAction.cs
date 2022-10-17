namespace SimpleVHD;

/// <summary>
/// 작업
/// </summary>
public enum DoAction {
    /// <summary>
    /// 아무것도 안함
    /// </summary>
    DoNothing,
    /// <summary>
    /// 백업
    /// </summary>
    DoBackup,
    /// <summary>
    /// 복원
    /// </summary>
    DoRestore,
    /// <summary>
    /// 초기화
    /// </summary>
    DoRevert,
    /// <summary>
    /// 병합
    /// </summary>
    DoMerge,
    /// <summary>
    /// 확장
    /// </summary>
    DoExpand,
    /// <summary>
    /// 축소
    /// </summary>
    DoShrink,
    /// <summary>
    /// 형식 변환
    /// </summary>
    DoConvertType,
    /// <summary>
    /// 포맷 변환
    /// </summary>
    DoConvertFormat,
    /// <summary>
    /// 스타일 전환
    /// </summary>
    DoSwitchStyle,
    /// <summary>
    /// 부모 VHD 부팅
    /// </summary>
    DoParentBoot,
    /// <summary>
    /// 재구축
    /// </summary>
    DoRebuild,
    /// <summary>
    /// 제거
    /// </summary>
    DoUninstall
}