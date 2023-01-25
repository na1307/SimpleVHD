namespace SimpleVHD.PEAction.Actions;

internal class Revert : Action {
    public Revert() {
        Name = "초기화";
        DifferentialOnly = true;
        AfterRevert = true;
        Shutdown = PVConfig.Instance.GetShutdown(DoAction.DoRevert);
    }
}