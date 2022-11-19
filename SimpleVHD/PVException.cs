using System.Runtime.Serialization;

namespace SimpleVHD;

public abstract class PVException : Exception {
    protected PVException(string message) : base(message) { }
    protected PVException(string message, Exception innerException) : base(message, innerException) { }
    protected PVException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}