namespace ProjectV.Processor.Actions;

internal class RebuildProcessor : ActionProcessor {
    protected override string Name => "자식 VHD 재구축";
    protected sealed override bool DifferentialOnly => true;
    protected sealed override bool AfterRebuild => true;
    protected sealed override bool AfterRevert => true;
}