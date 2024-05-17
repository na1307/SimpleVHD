namespace SimpleVhd;

public static class Checker {
    public static void Check() {
        var baseDirectory = AppContext.BaseDirectory;

        if (new DriveInfo(baseDirectory).DriveType != DriveType.Fixed) {
            throw new CheckException("SimpleVhd는 고정 드라이브에 위치해야 합니다.");
        }

        var requires = new string[] { "Boot\\x64.wim", "Boot\\arm64.wim", "Boot\\boot.sdi" };
        var requireNotFound = requires.Where(f => !File.Exists(Path.Combine(baseDirectory, "..", f)));

        if (requireNotFound.Any()) {
            throw new CheckException($"{requireNotFound.Select(f => Path.GetFileName(f)).ToJoinedString(", ")} 파일을 찾을 수 없습니다.");
        }
    }
}
