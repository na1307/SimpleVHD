using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProjectV;

/// <summary>
/// bcdedit 함수 모음
/// </summary>
public static class BcdEdit {
    /// <summary>
    /// bcdedit 작업을 실행하고 그 출력을 반환
    /// </summary>
    /// <param name="arg">bcdedit 명령으로 실행할 매개 변수</param>
    /// <returns>실행한 bcdedit 명령의 전체 출력</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="BcdException"></exception>
    public static string ProcessBcdEdit(string arg) {
        if (string.IsNullOrWhiteSpace(arg)) throw new ArgumentException($"'{nameof(arg)}'은(는) null이거나 공백일 수 없습니다.", nameof(arg));

        using Process bcdedit = new() {
            StartInfo = new() {
                FileName = "bcdedit.exe",
                Arguments = arg,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            }
        };

        bcdedit.Start();
        bcdedit.WaitForExit();

        var x = bcdedit.StandardOutput.ReadToEnd();

        return bcdedit.ExitCode == 0 ? x : throw new BcdException(x);
    }

    /// <summary>
    /// 여러 bcdedit 작업을 실행함
    /// </summary>
    /// <param name="args">bcdedit 명령으로 실행할 매개 변수</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void ProcessBcdEdit(params string[] args) {
        if (args == null) throw new ArgumentNullException(nameof(args));

        foreach (var arg in args) ProcessBcdEdit(arg);
    }

    /// <summary>
    /// bcdedit 작업을 실행하고 그 출력에서 정규식 일치를 반환
    /// </summary>
    /// <param name="arg">bcdedit 명령으로 실행할 매개 변수</param>
    /// <param name="pattern">찾을 정규식 구문</param>
    /// <returns>정규식 <see cref="Match"/></returns>
    /// <exception cref="BcdException"></exception>
    public static Match BcdEditRegex(string arg, string pattern) {
        var output = ProcessBcdEdit(arg);
        var m = Regex.Match(output, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

        return m.Success ? m : throw new BcdException(output);
    }

    /// <summary>
    /// bcdedit 작업을 실행하고 그 출력에서 정규식 일치 컬렉션을 반환
    /// </summary>
    /// <param name="arg">bcdedit 명령으로 실행할 매개 변수</param>
    /// <param name="pattern">찾을 정규식 구문</param>
    /// <returns>정규식 <see cref="MatchCollection"/></returns>
    /// <exception cref="BcdException"></exception>
    public static MatchCollection BcdEditRegexAll(string arg, string pattern) {
        var output = ProcessBcdEdit(arg);
        var m = Regex.Matches(output, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

        return m.Count > 0 ? m : throw new BcdException(output);
    }

    public class BcdException : PVException {
        private const string message = "bcdedit 작업 도중 오류가 발생했습니다.";

        public BcdException() : base(message) { }
        public BcdException(string reason) : base(message + "\r\n\r\n" + reason) { }
        public BcdException(string reason, Exception innerException) : base(message + "\r\n\r\n" + reason, innerException) { }
    }
}