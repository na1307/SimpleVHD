namespace SimpleVHD.PEAction.Actions;

internal class Rebuild : Action {
    protected override string Name => "자식 VHD 재구축";

    public Rebuild() {
        DifferentialOnly = true;
        AfterRebuild = true;
        AfterRevert = true;
    }
}