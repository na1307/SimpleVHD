using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel;

public sealed partial class HomeScreen {
    public HomeScreen() => InitializeComponent();

    private void BackupButton_Click(object sender, RoutedEventArgs e) {
        ((App)Application.Current).MWindow!.Screen =
            new OperationScreen.Builder()
            .SetBackScreen(this)
            .SetOperationType(OperationType.Backup)
            .SetOperationName("백업")
            .SetOperationDescription("VHD를 백업합니다.")
            .SetAdditionalDescription("설명")
            .SetSymbol(Symbol.Accept)
            .Build();
    }

    private void RestoreButton_Click(object sender, RoutedEventArgs e) {
        throw new NotImplementedException();
    }
}
