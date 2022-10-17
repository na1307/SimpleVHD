namespace SimpleVHD.PEAction;

internal abstract class PVActionException : PVException {
    protected PVActionException(string message) : base(message) { }
    protected PVActionException(string message, Exception innerException) : base(message, innerException) { }
}