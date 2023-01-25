namespace SimpleVHD.PEAction.Actions;

internal class Rebuild : Action {
    public Rebuild() {
        Name = "자식 VHD 재구축";
        DifferentialOnly = true;
        AfterRebuild = true;
        AfterRevert = true;
    }
}