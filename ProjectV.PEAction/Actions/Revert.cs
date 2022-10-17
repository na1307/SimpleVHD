namespace ProjectV.PEAction.Actions;

internal class Revert : Action {
    protected override string Name => "초기화";
    protected sealed override bool DifferentialOnly => true;
    protected sealed override bool AfterRevert => true;
    protected override bool Shutdown => PVConfig.Instance[DoAction.DoRevert];
}