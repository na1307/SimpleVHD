using Microsoft.UI.Xaml;
using SimpleVhd.Installer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.Installer.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class InstallingPage {
    public InstallingPage() => InitializeComponent();

    public override InstallingPageViewModel ViewModel { get; } = new();

    private async void StepPage_Loaded(object sender, RoutedEventArgs e) => await ViewModel.ProcessAsync();
}
