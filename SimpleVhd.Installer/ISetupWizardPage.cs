namespace SimpleVhd.Installer;

public interface ISetupWizardPage {
    string Title { get; }
    string Description { get; }
    UserControl Panel { get; }
}
