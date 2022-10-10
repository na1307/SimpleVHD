namespace ProjectV.ControlPanel;

public abstract class SubScreen : Screen {
    private readonly Screen backScreen;

    protected SubScreen(Screen backScreen) => this.backScreen = backScreen;

    protected void GoBack() => ((MainWindow)Application.Current.MainWindow).Screen = backScreen;
}