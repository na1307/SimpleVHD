namespace SimpleVHD.PEAction.Actions;

internal class SwitchSimpleStyle : SwitchStyle {
    protected override void RunCore() {
        PVConfig.Instance.Temp = OperatingStyle.Simple.ToString();
        base.RunCore();
    }
}