using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel;

public abstract class Screen : ContentControl {
    static Screen() {
        DependencyProperty.Register(nameof(IsTabStopProperty), typeof(bool), typeof(Screen), new PropertyMetadata(false));
    }
}
