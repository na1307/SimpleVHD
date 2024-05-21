using Microsoft.UI.Xaml.Controls;

namespace SimpleVhd.ControlPanel.Installer;

public abstract class StepPage : Page {
    public abstract string Title { get; }
    public abstract string Description { get; }
}
