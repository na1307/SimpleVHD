using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel;

public sealed partial class OperationScreen {
    private OperationScreen(Screen backScreen, OperationType operationType, string operationName, string operationDescription, string additionalDescription, Symbol symbol) : base(backScreen) {
        InitializeComponent();
        OperationName = operationName;
        OperationType = operationType;
        OperationDescription = operationDescription;
        AdditionalDescription = additionalDescription;
        Symbol = symbol;
    }

    public OperationType OperationType { get; }
    public string OperationName { get; }
    public string OperationDescription { get; }
    public string AdditionalDescription { get; }
    public Symbol Symbol { get; }

    private void BackButton_Click(object sender, RoutedEventArgs e) => GoBack();

    private void StartButton_Click(object sender, RoutedEventArgs e) {
        throw new NotImplementedException();
    }

    public sealed class Builder {
        private Screen? _backScreen;
        private OperationType _operationType;
        private string _operationName = string.Empty;
        private string _operationDescription = string.Empty;
        private string _additionalDescription = string.Empty;
        private Symbol _symbol;

        public Builder SetBackScreen(Screen backScreen) {
            _backScreen = backScreen;
            return this;
        }

        public Builder SetOperationType(OperationType operationType) {
            _operationType = operationType;
            return this;
        }

        public Builder SetOperationName(string operationName) {
            _operationName = operationName;
            return this;
        }
        public Builder SetOperationDescription(string operationDescription) {
            _operationDescription = operationDescription;
            return this;
        }

        public Builder SetAdditionalDescription(string additionalDescription) {
            _additionalDescription = additionalDescription;
            return this;
        }

        public Builder SetSymbol(Symbol symbol) {
            _symbol = symbol;
            return this;
        }

        public OperationScreen Build() {
            if (_backScreen == null) {
                throw new InvalidOperationException("BackScreen을 지정해야 합니다.");
            }

            return new(_backScreen, _operationType, _operationName, _operationDescription, _additionalDescription, _symbol);
        }
    }
}
