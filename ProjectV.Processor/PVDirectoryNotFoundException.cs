#nullable enable
namespace ProjectV.Processor;

internal class PVDirectoryNotFoundException : PVProcessorException {
    public PVDirectoryNotFoundException() : base("설정 파일을 찾지 못하였습니다.") { }
    public PVDirectoryNotFoundException(string message) : base(message) { }
    public PVDirectoryNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}