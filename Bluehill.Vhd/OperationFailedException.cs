namespace Bluehill.Vhd;

public sealed class OperationFailedException : Exception {
    public OperationFailedException() : base("Operation failed.") { }
    public OperationFailedException(string message) : base(message) { }
    public OperationFailedException(string message, Exception innerException) : base(message, innerException) { }
}
