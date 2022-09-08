#nullable enable
namespace ProjectV.Processor;

internal class BackupDirectoryNotFoundException : PVProcessorException {
    public BackupDirectoryNotFoundException() : base("백업 디렉토리를 찾지 못하였습니다.") { }
    public BackupDirectoryNotFoundException(string message) : base(message) { }
    public BackupDirectoryNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}