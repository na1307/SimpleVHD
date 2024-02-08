using Json.Schema;
using System.Reflection;
using System.Text.Json;

namespace SimpleVhd;

public static class BaseChecker {
    private static readonly string[] array = ["Boot\\x64.wim", "Boot\\arm64.wim", "Boot\\boot.sdi"];

    public static void Check(bool checkSchema) {
        IEnumerable<string> drvs = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed && Directory.Exists(Path.Combine(d.Name, DirName))).Select(d => d.Name);
        var dir = drvs.Any() ? drvs.First() + DirName : throw new CheckException("아무 드라이브의 루트에 " + DirName + " 폴더가 없습니다.");
        IEnumerable<string> requires = array.Where(f => !File.Exists(Path.Combine(dir, f)));

        if (requires.Any()) {
            throw new CheckException(Path.GetFileName(requires.First()) + " 파일이 없습니다.");
        }

        if (!checkSchema) {
            return;
        }

        using Stream ss = Assembly.GetExecutingAssembly().GetManifestResourceStream("SimpleVhd.Settings.schema.json")!;
        using StreamReader sr = new(ss);

        try {
            if (!JsonSchema.FromText(sr.ReadToEnd()).Evaluate(JsonDocument.Parse(File.ReadAllBytes(AppContext.BaseDirectory + "..\\" + SettingsFileName))).IsValid) {
                throw new CheckException("설정 파일이 올바르지 않습니다.");
            }
        } catch (FileNotFoundException fnfex) {
            throw new CheckException("설정 파일을 찾을 수 없습니다.", fnfex);
        }
    }
}
