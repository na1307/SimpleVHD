namespace ProjectV.Processor.Actions;

internal class RevertProcessor : ActionProcessor {
    protected override string Name => "초기화";
    protected sealed override bool DifferentialOnly => true;
    protected sealed override bool AfterRevert => true;
    protected override bool Shutdown => PVConfig.Instance[DoAction.DoRevert];
}