using Microsoft.UI.Xaml.Controls;
using SimpleVhd.Installer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.Installer.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class VhdTypePage {
    public VhdTypePage() => InitializeComponent();

    public override VhdTypePageViewModel ViewModel { get; } = new();

    private void VhdTypeRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        => ViewModel.VhdType = (VhdType)((RadioButtons)sender).SelectedIndex;
}
