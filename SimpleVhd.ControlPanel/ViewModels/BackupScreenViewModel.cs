using Bluehill.Bcd;
using Microsoft.UI.Xaml;
using SimpleVhd.ControlPanel.Views;

namespace SimpleVhd.ControlPanel.ViewModels;

public sealed class BackupScreenViewModel(IMainWindow window, IScreen previousScreen) : OperationScreenViewModel(window, previousScreen) {
    public override string Title => "백업";
    public override string Description => "원본 VHD를 백업합니다.";
    public override string Icon => "\xEDA2";

    protected override void ProcessButton() {
        var settings = Settings.Instance;
        settings.OperationType = OperationType.Backup;
        settings.OperationTarget = settings.Instances.IndexOf(settings.CurrentInstance!);
        var bootmgr = BcdStore.SystemStore.OpenObject(WellKnownGuids.BootMgr);
        var pe = BcdStore.SystemStore.OpenObject(settings.PEGuid);
        bootmgr.SetObjectListElement(BcdElementType.BcdBootMgrBootSequence, pe);
        RestartHelper.Restart();
        Application.Current.Exit();
    }
}
