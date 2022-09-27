#nullable enable
namespace ProjectV.Processor.Actions;

internal class EmergencyRestoreProcessor : RestoreProcessor {
    protected override VhdType VType => VhdType.Expandable;

    public EmergencyRestoreProcessor() : base("응급 복원") { }

    protected override void DoProcessCore() {
        base.DoProcessCore();
        PVConfig.Instance.VhdType = VhdType.Expandable;
    }
}