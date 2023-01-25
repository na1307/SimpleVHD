namespace SimpleVHD.ControlPanel;

public abstract class SwitchScreen : SubScreen {
    protected SwitchScreen(Screen backscreen) : base(backscreen) { }

    protected static void DoProcess<T>(string name, DoAction action, T value) where T : struct, Enum {
        if (MessageBox.Show("전환 작업 시 현재 차등 스타일을 사용 중인 경우 모든 변경분이 초기화됩니다.\r\n\r\n언제든지 다시 다른 " + name.EuRo() + " 변경하실 수 있습니다. 계속하시겠습니까?", "경고", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes) return;

        PVConfig.Instance.Action = action;
        PVConfig.Instance.Temp = value.ToString();

        ProcessBcdEdit("/bootsequence " + PVConfig.Instance.GetGuid(GuidType.PE));
        App.SystemRestart();
    }
}