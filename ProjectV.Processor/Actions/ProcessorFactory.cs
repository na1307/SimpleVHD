namespace ProjectV.Processor.Actions;

internal static class ProcessorFactory {
    public static ActionProcessor Create(DoAction action) => action switch {
        DoAction.DoBackup => new BackupProcessor(),
        DoAction.DoRestore => new RestoreProcessor(),
        DoAction.DoRevert => new RevertProcessor(),
        DoAction.DoMerge => new MergeProcessor(),
        DoAction.DoExpand => new ExpandProcessor(),
        DoAction.DoShrink => new ShrinkProcessor(),
        DoAction.DoConvertType => new ConvertTypeProcessor(),
        DoAction.DoConvertFormat => new ConvertFormatProcessor(),
        DoAction.DoSwitchStyle => new SwitchStyleProcessor(),
        DoAction.DoRebuild => new RebuildProcessor(),
        _ => throw new ArgumentException("이 작업은 지원되지 않습니다.", nameof(action))
    };
}