namespace ProjectV.ControlPanel;

public static class SubScreenFactory {
    public static SubScreen Create(Screen parent, PanelAction action) => action switch {
        PanelAction.DoConvertType => new TypeScreen(parent),
        PanelAction.DoConvertFormat => new FormatScreen(parent),
        PanelAction.DoSwitchStyle => new StyleScreen(parent),
        _ => new ActionScreen(parent, action),
    };
}