namespace SimpleVhd;

public class SimpleVhdException : Exception {
    public SimpleVhdException(string message) : base(message) { }
    public SimpleVhdException(string message, Exception innerException) : base(message, innerException) { }
}
