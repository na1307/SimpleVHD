namespace ProjectV.Installer;

internal class RequirementNotFoundException : PVInstallerException {
    private const string message = " 파일을 찾을 수 없습니다.\r\n\r\n혹시 설치 패키지의 압축을 백업 드라이브의 루트 폴더가 아닌 하위 폴더에\r\n풀지는 않았는지 확인해 보시길 바랍니다.";

    public RequirementNotFoundException() : base("필수" + message) { }
    public RequirementNotFoundException(string file) : base(file + message) { }
    public RequirementNotFoundException(string file, Exception innerException) : base(file + message, innerException) { }
}