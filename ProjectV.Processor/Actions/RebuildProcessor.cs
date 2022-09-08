#nullable enable
namespace ProjectV.Processor.Actions;

internal class RebuildProcessor : ActionProcessor {
    protected sealed override bool DifferentialOnly => true;
    protected sealed override bool AfterRebuild => true;
    protected sealed override bool AfterRevert => true;

    public RebuildProcessor() : base("자식 VHD 재구축") { }
}