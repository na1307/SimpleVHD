namespace SimpleVHD;

public abstract class PVException : Exception {
    protected PVException(string message) : base(message) { }
    protected PVException(string message, Exception innerException) : base(message, innerException) { }
}