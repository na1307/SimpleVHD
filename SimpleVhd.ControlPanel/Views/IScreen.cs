using Microsoft.UI.Xaml;

namespace SimpleVhd.ControlPanel.Views;

public interface IScreen {
    Screen Self { get; }
    XamlRoot XamlRoot { get; }
}
