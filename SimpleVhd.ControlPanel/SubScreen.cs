using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel;

public abstract class SubScreen : Screen {
    private readonly Screen backScreen;

    protected SubScreen(Screen backScreen) => this.backScreen = backScreen;

    protected void GoBack() => ((App)Application.Current).MWindow!.Screen = backScreen;
}
