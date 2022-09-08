#nullable enable
namespace ProjectV.Processor;

internal class VhdFileNotFoundException : PVProcessorException {
    public VhdFileNotFoundException() : base("원본 VHD 파일을 찾지 못하였습니다.") { }
    public VhdFileNotFoundException(string message) : base(message) { }
    public VhdFileNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}