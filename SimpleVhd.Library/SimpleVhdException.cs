namespace SimpleVhd;

public abstract class SimpleVhdException : Exception {
    protected SimpleVhdException(string message) : base(message) { }
    protected SimpleVhdException(string message, Exception innerException) : base(message, innerException) { }
}
