namespace Bluehill.Vhd;

public sealed class VhdOperationFailedException : Exception {
    public VhdOperationFailedException() : base("Operation failed.") { }
    public VhdOperationFailedException(string message) : base(message) { }
    public VhdOperationFailedException(string message, Exception innerException) : base(message, innerException) { }
}
