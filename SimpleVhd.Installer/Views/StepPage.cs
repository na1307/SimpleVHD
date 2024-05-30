using Microsoft.UI.Xaml.Controls;

namespace SimpleVhd.Installer.Views;

public abstract class StepPage : Page {
    public abstract string Title { get; }
    public abstract string Description { get; }
}
