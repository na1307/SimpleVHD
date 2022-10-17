namespace ProjectV.PEAction.Actions;

internal class EmergencyRestore : Restore {
    protected override string Name => "응급 복원";
    protected override bool Shutdown => false;
    protected override VhdType VType => VhdType.Expandable;

    protected override void RunCore() {
        base.RunCore();
        PVConfig.Instance.VhdType = VhdType.Expandable;
    }
}