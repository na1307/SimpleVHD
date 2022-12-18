namespace SimpleVHD.ControlPanel;

public abstract class Screen : ContentControl {
    static Screen() {
        FocusableProperty.OverrideMetadata(typeof(Screen), new FrameworkPropertyMetadata(false));
        System.Windows.Input.KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(Screen), new FrameworkPropertyMetadata(false));
    }
}