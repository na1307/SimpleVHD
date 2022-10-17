namespace SimpleVHD.Installer;

internal abstract class PVInstallerException : PVException {
    protected PVInstallerException(string message) : base(message) { }
    protected PVInstallerException(string message, Exception innerException) : base(message, innerException) { }
}