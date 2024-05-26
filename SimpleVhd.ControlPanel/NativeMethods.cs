using System.Runtime.InteropServices;

namespace SimpleVhd.ControlPanel;

internal static class NativeMethods {
    [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    public static extern int MessageBoxW(nint hWnd, string? text, string? caption, uint type);
}
