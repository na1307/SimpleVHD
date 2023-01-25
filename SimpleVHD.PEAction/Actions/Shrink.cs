using System.Text.RegularExpressions;

namespace SimpleVHD.PEAction.Actions;

internal class Shrink : Action {
    public Shrink() {
        Name = "VHD 축소";
        NeedBackup = true;
        AfterRebuild = true;
        AfterRevert = true;
    }

    protected override void RunCore() {
        var output = ProcessDiskpartOutput($"select vdisk file \"{VhdDir}{PVConfig.Instance.VhdFile}\"", "detail vdisk");
        var mx = Regex.Match(output, @"가상 크기:\s+(?<size>.+)$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        var mc = Regex.Match(output, @"물리적 크기:\s+(?<size>.+)$", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        if (!mx.Success || !mc.Success) throw new ProcessFailedException("diskpart 작업이 실패했습니다.\r\n\r\n" + output);

        var nsz = getNewSize(mx.Groups["size"].Value, mc.Groups["size"].Value);

        const string testvhd = "shrink.vhd";
        const string tempwim = "shrink.wim";

        try {
            ProcessDiskpart($"create vdisk file \"{BackupDir}{testvhd}\" maximum {nsz} type expandable");
        } catch (ProcessFailedException ex) {
            throw new ProcessFailedException("작업을 위한 공간이 부족합니다.", ex);
        } finally {
            File.Delete(BackupDir + testvhd);
        }

        ProcessDiskpart($"select vdisk file \"{VhdDir}{PVConfig.Instance.VhdFile}\"", "attach vdisk");

        var i = 1;

        while (true) {
            try {
                ProcessDiskpart($"select vdisk file \"{VhdDir}{PVConfig.Instance.VhdFile}\"", "select partition " + i, "assign letter t");
            } catch (ProcessFailedException) {
                i++;
                continue;
            }

            break;
        }

        using (Process imagex = new() {
            StartInfo = new() {
                FileName = "imagex.exe",
                Arguments = $"/capture /compress fast t: \"{BackupDir}{tempwim}\" \"Windows\"",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden
            }
        }) {
            imagex.Start();
            imagex.WaitForExit();

            ProcessDiskpart($"select vdisk file \"{VhdDir}{PVConfig.Instance.VhdFile}\"", "detach vdisk");

            if (imagex.ExitCode != 0) throw new ProcessFailedException("imagex 작업이 실패했습니다.");
        }

        File.Delete(VhdDir + PVConfig.Instance.VhdFile);
        ProcessDiskpart($"create vdisk file \"{VhdDir}{PVConfig.Instance.VhdFile}\" maximum {nsz} type {PVConfig.Instance.VhdType}", "attach vdisk", "create partition msr size 16 noerr", "create partition primary", "format fs ntfs quick", "assign letter v");

        using (Process imagex = new() {
            StartInfo = new() {
                FileName = "imagex.exe",
                Arguments = $"/apply \"{BackupDir}{tempwim}\" 1 v:",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden
            }
        }) {
            imagex.Start();
            imagex.WaitForExit();

            File.Delete(BackupDir + tempwim);
            ProcessDiskpart($"select vdisk file \"{VhdDir}{PVConfig.Instance.VhdFile}\"", "detach vdisk");

            if (imagex.ExitCode != 0) throw new ProcessFailedException("imagex 작업이 실패했습니다. VHD를 복원해 주세요.");
        }

        static ulong getNewSize(string maxsize, string currentsize) {
            FormInput dlg = new("축소", $"현재 최대 크기는 {maxsize.Trim()} 이고, 현재 크기는 {currentsize.Trim()} 입니다.\r\n\r\n새로운 최대 크기를 MB 단위로 입력하세요.\r\n새로운 최대 크기는 파일을 모두 옮길 수 있을 만큼 커야 합니다.\r\n그러니 가급적 현재 크기보다 크게 잡는 것이 좋습니다. (1GB = 1024MB)");

            while (true) {
                switch (dlg.ShowDialog()) {
                    case DialogResult.OK:
                        if (!ulong.TryParse(dlg.Input, out var r)) {
                            MessageBox.Show("숫자만 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                        if (MessageBox.Show($"최대 크기를 {r} MB로 축소합니다.\r\n\r\n새로운 크기가 너무 작거나 백업 공간이 부족하면 작업은 실패합니다.\r\n\r\n작업이 중간에 실패하면 복원을 진행해야만 복구할 수 있습니다.\r\n\r\n정말로 작업을 시작하시겠습니까?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes) continue;

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