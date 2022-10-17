namespace ProjectV.PEAction.Actions;

internal static class ActionFactory {
    public static Action Create(DoAction action) => action switch {
        DoAction.DoBackup => new Backup(),
        DoAction.DoRestore => new Restore(),
        DoAction.DoRevert => new Revert(),
        DoAction.DoMerge => new Merge(),
        DoAction.DoExpand => new Expand(),
        DoAction.DoShrink => new Shrink(),
        DoAction.DoConvertType => new ConvertType(),
        DoAction.DoConvertFormat => new ConvertFormat(),
        DoAction.DoSwitchStyle => new SwitchStyle(),
        DoAction.DoRebuild => new Rebuild(),
        _ => throw new ArgumentException("이 작업은 지원되지 않습니다.", nameof(action))
    };
}