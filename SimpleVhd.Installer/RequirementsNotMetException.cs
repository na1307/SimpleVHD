namespace SimpleVhd.Installer;

internal sealed class RequirementsNotMetException : SimpleVhdException {
    public RequirementsNotMetException() : base("요구 사항이 맞지 않습니다.") { }
    public RequirementsNotMetException(string message) : base(message) { }
}
