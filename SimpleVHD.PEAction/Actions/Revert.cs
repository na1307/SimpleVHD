namespace SimpleVHD.PEAction.Actions;

internal class Revert : Action {
    protected override string Name => "초기화";

    public Revert() {
        DifferentialOnly = true;
        AfterRevert = true;
        Shutdown = PVConfig.Instance.GetShutdown(DoAction.DoRevert);
    }
}