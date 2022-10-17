namespace ProjectV.ControlPanel;

public enum PanelAction {
    DoPEBoot = DoAction.DoNothing,
    DoBackup = DoAction.DoBackup,
    DoRestore = DoAction.DoRestore,
    DoRevert = DoAction.DoRevert,
    DoMerge = DoAction.DoMerge,
    DoExpand = DoAction.DoExpand,
    DoShrink = DoAction.DoShrink,
    DoConvertType = DoAction.DoConvertType,
    DoConvertFormat = DoAction.DoConvertFormat,
    DoSwitchStyle = DoAction.DoSwitchStyle,
    DoParentBoot = DoAction.DoParentBoot,
    DoUninstall = DoAction.DoUninstall
}