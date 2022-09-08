#nullable enable
namespace ProjectV.Processor.Actions;

internal class RevertProcessor : ActionProcessor {
    protected sealed override bool DifferentialOnly => true;
    protected sealed override bool AfterRevert => true;

    public RevertProcessor() : base("초기화", Config[ShutdownType.Revert]) { }
}