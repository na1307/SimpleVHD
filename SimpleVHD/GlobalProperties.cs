namespace SimpleVHD;

/// <summary>
/// 전역 속성 모음
/// </summary>
public static class GlobalProperties {
    /// <summary>
    /// 현재 차등 스타일을 사용 중인지 여부
    /// </summary>
    public static bool IsDifferentialStyle => PVConfig.Instance.OperatingStyle is OperatingStyle.DifferentialManual or OperatingStyle.DifferentialAuto;

    /// <summary>
    /// 백업 파일이 존재하는지 여부
    /// </summary>
    public static bool BackupExists => DriveInfo.GetDrives().Any(drv => drv.CheckFixed() && (File.Exists(drv.Name + BackupDirName + Path.DirectorySeparatorChar.ToString() + PVConfig.Instance.VhdFile) || File.Exists(drv.Name + DirName + Path.DirectorySeparatorChar.ToString() + IncludedBackupDirName + Path.DirectorySeparatorChar.ToString() + PVConfig.Instance.VhdFile)));

    /// <summary>
    /// 작업 후 다시 시작하는 대신 시스템을 종료할 수 있는 작업
    /// </summary>
    public static IEnumerable<DoAction> ShutdownAction {
        get {
            yield return DoAction.DoBackup;
            yield return DoAction.DoRestore;
            yield return DoAction.DoRevert;
            yield return DoAction.DoMerge;
        }
    }
}