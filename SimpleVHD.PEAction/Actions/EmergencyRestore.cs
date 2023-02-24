namespace SimpleVHD.PEAction.Actions;

internal class EmergencyRestore : Restore {
    protected override string Name => "응급 복원";

    public EmergencyRestore() {
        Shutdown = false;
        VType = VhdType.Expandable;
    }

    protected override void RunCore() {
        base.RunCore();
        PVConfig.Instance.VhdType = VhdType.Expandable;
    }
}