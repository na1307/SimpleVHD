namespace SimpleVHD.PEAction.Actions;

internal class EmergencyRestore : Restore {
    public EmergencyRestore() {
        Name = "응급 복원";
        Shutdown = false;
        VType = VhdType.Expandable;
    }

    protected override void RunCore() {
        base.RunCore();
        PVConfig.Instance.VhdType = VhdType.Expandable;
    }
}