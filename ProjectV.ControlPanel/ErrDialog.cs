#nullable enable
global using static ProjectV.ControlPanel.ErrDialog;

namespace ProjectV.ControlPanel;

public static class ErrDialog {
    public static void ErrMsg(string message, bool exit) {
        ErrMsg(message);
        if (exit) Application.Current.Shutdown(1);
    }

    public static MessageBoxResult ErrMsg(string message) => ErrMsg(message, MessageBoxButton.OK);
    public static MessageBoxResult ErrMsg(string message, MessageBoxButton button) => MessageBox.Show(message, "오류", button, MessageBoxImage.Error);
}