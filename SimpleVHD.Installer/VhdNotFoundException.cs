namespace SimpleVHD.Installer;

internal class VhdNotFoundException : PVInstallerException {
    private const string m = "현재 실행 중인 VHD 파일을 찾을 수 없습니다.\r\n\r\nSimpleVHD를 설치할 윈도우로 부팅했는지 확인하시기 바랍니다.";

    public VhdNotFoundException() : base(m) { }
    public VhdNotFoundException(string message) : base(message) { }
    public VhdNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public VhdNotFoundException(Exception innerException) : base(m, innerException) { }
}