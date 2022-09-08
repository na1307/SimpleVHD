namespace ProjectV;

public enum ShutdownType {
    Backup = DoAction.DoBackup,
    Restore = DoAction.DoRestore,
    Revert = DoAction.DoRevert,
    Merge = DoAction.DoMerge
}