#nullable enable
using System.Text.RegularExpressions;

namespace ProjectV.Processor.Actions;

internal class ExpandProcessor : ActionProcessor {
    protected override string Name => "VHD 확장";
    protected sealed override bool AfterRebuild => true;
    protected sealed override bool AfterRevert => true;

    protected override void DoProcessCore() {
        var output = ProcessDiskpartOutput($"select vdisk file \"{VhdDir}{PVConfig.Instance.VhdFile}\"", "detail vdisk");
        var m = Regex.Match(output, @"가상 크기:\s+(?<size>.+)$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        string sz = m.Success ? m.Groups["size"].Value : throw new ProcessFailedException("diskpart 작업이 실패했습니다.\r\n\r\n" + output);

        ProcessDiskpart($"select vdisk file \"{VhdDir}{PVConfig.Instance.VhdFile}\"", "expand vdisk maximum " + getNewSize());

        ulong getNewSize() {
            FormInput dlg = new("확장", $"현재 최대 크기는 {sz.Trim()} 입니다.\r\n\r\n새로운 최대 크기를 MB 단위로 입력하세요.\r\n새로운 최대 크기는 반드시 기존 크기보다 커야 합니다. (1GB = 1024MB)");

            while (true) {
                switch (dlg.ShowDialog()) {
                    case DialogResult.OK:
                        if (!ulong.TryParse(dlg.Input, out var r)) {
                            MessageBox.Show("숫자만 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                        if (MessageBox.Show($"최대 크기를 {r} MB로 확장합니다.\r\n\r\n다시 한 번 기존 크기보다 큰지, 정확하게 입력하였는지 확인해보시길 바랍니다.\r\n\r\n정말로 작업을 시작하시겠습니까?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes) continue;

                        return r;

                    case DialogResult.Cancel:
                        throw new ProcessFailedException("작업을 취소했습니다.");

                    default:
                        throw new ProcessFailedException("알 수 없는 오류");
                }
            }
        }
    }
}