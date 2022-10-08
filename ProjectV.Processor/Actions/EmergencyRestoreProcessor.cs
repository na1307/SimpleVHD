namespace ProjectV.Processor.Actions;

internal class EmergencyRestoreProcessor : RestoreProcessor {
    protected override string Name => "응급 복원";
    protected override bool Shutdown => false;
    protected override VhdType VType => VhdType.Expandable;

    protected override void DoProcessCore() {
        base.DoProcessCore();
        PVConfig.Instance.VhdType = VhdType.Expandable;
    }
}