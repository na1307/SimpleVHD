namespace SimpleVhd;

public sealed class CheckException : SimpleVhdException {
    public CheckException(string message) : base(message) { }
    public CheckException(string message, Exception innerException) : base(message, innerException) { }
}
