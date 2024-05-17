namespace SimpleVhd;

public static class Checker {
    public static void Check() {
        if (!Array.Exists(DriveInfo.GetDrives(), predicate)) {
            throw new CheckException($"{SVDirName} 폴더를 찾을 수 없습니다.{Environment.NewLine}{Environment.NewLine}드라이브의 루트에 {SVDirName} 폴더가 있고 필요한 파일들이 모두 있는지 확인하세요.");
        }

        static bool predicate(DriveInfo drv) {
            var isFixed = drv.DriveType == DriveType.Fixed;
            var dirs = drv.RootDirectory.EnumerateDirectories().Where(dir => dir.Name == SVDirName);
            var isSvDirExists = dirs.Any();

            return isFixed && isSvDirExists && isFilesExists(dirs.First(), ["Boot\\x64.wim", "Boot\\arm64.wim", "Boot\\boot.sdi"]);

            static bool isFilesExists(DirectoryInfo directory, IEnumerable<string> fileNames) {
                var value = true;

                foreach (var fileName in fileNames) {
                    value = value && File.Exists(Path.Combine(directory.FullName, fileName));
                }

                return value;
            }
        }
    }
}
