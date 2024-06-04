namespace SimpleVhd;

public sealed class CheckException : SimpleVhdException {
    internal CheckException(string message) : base(message) { }
    internal CheckException(string message, Exception innerException) : base(message, innerException) { }
}
