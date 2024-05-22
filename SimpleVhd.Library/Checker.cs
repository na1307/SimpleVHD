using Json.Schema;
using System.Reflection;
using System.Text.Json;

namespace SimpleVhd;

public static class Checker {
    public static void Check() {
        checkSvPath();

        if (SVPath == string.Empty) {
            throw new CheckException($"{SVDirName} 폴더를 찾을 수 없습니다.{Environment.NewLine}{Environment.NewLine}드라이브의 루트에 {SVDirName} 폴더가 있고 필요한 파일들이 모두 있는지 확인하세요.");
        }

        if (GetSystemVhdPath() == string.Empty) {
            throw new CheckException($"현재 가상 디스크로 부팅하지 않았습니다.{Environment.NewLine}{Environment.NewLine}가상 디스크로 부팅한 후 다시 시도하세요.");
        }

        static void checkSvPath() {
            foreach (var drv in DriveInfo.GetDrives()) {
                if (drv.DriveType == DriveType.Fixed) {
                    var dirs = drv.RootDirectory.EnumerateDirectories().Where(dir => dir.Name == SVDirName);
                    var isSvDirExists = dirs.Any();

                    if (isSvDirExists && !Array.Exists(["Boot\\x64.wim", "Boot\\arm64.wim", "Boot\\boot.sdi"], f => !File.Exists(Path.Combine(dirs.First().FullName, f)))) {
                        SVPath = dirs.First().FullName;
                        return;
                    }
                }
            }
        }
    }

    public static async Task CheckSettingsJsonAsync() {
        using var ss = Assembly.GetExecutingAssembly().GetManifestResourceStream("SimpleVhd.Settings.schema.json")!;

        try {
            var schema = await JsonSchema.FromStream(ss);

            if (!schema.Evaluate(JsonDocument.Parse(File.ReadAllBytes(Path.Combine(SVPath, SettingsFileName)))).IsValid) {
                throw new CheckException("설정 파일이 올바르지 않습니다.");
            }
        } catch (FileNotFoundException fnfex) {
            throw new CheckException("설정 파일을 찾을 수 없습니다.", fnfex);
        }
    }
}
