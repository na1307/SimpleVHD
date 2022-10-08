namespace ProjectV.Processor;

internal abstract class PVProcessorException : PVException {
    protected PVProcessorException(string message) : base(message) { }
    protected PVProcessorException(string message, Exception innerException) : base(message, innerException) { }
}