using Microsoft.UI.Xaml.Controls;
using SimpleVhd.Installer.ViewModels;

namespace SimpleVhd.Installer.Views;

public abstract class StepPage : Page {
    public abstract StepPageViewModel ViewModel { get; }
}
