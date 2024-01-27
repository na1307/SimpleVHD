using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SimpleVhd;

[Obsolete("WMI로 대체해야 함")]
public static class BcdEdit {
    public static void ProcessBcdEdit(string arg) {
        if (string.IsNullOrWhiteSpace(arg)) throw new ArgumentException($"'{nameof(arg)}'은(는) null이거나 공백일 수 없습니다.", nameof(arg));

        using Process bcdedit = new() {
            StartInfo = {
                FileName = "bcdedit.exe",
                Arguments = arg,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        bcdedit.Start();
        bcdedit.WaitForExit();

        if (bcdedit.ExitCode != 0) throw new SimpleVhdException("BCD");
    }

    public static void ProcessBcdEdit(params string[] args) {
        ArgumentNullException.ThrowIfNull(args);
        Array.ForEach(args, ProcessBcdEdit);
    }

    public static string ProcessBcdEditOutput(string arg) {
        if (string.IsNullOrWhiteSpace(arg)) throw new ArgumentException($"'{nameof(arg)}'은(는) null이거나 공백일 수 없습니다.", nameof(arg));

        using Process bcdedit = new() {
            StartInfo = {
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

        return bcdedit.ExitCode == 0 ? x : throw new SimpleVhdException("BCD");
    }

    public static Match BcdEditRegex(string arg, string pattern) {
        var output = ProcessBcdEditOutput(arg);
        var m = Regex.Match(output, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

        return m.Success ? m : throw new SimpleVhdException(output);
    }

    public static MatchCollection BcdEditRegexAll(string arg, string pattern) {
        var output = ProcessBcdEditOutput(arg);
        var m = Regex.Matches(output, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

        return m.Count > 0 ? m : throw new SimpleVhdException(output);
    }
}