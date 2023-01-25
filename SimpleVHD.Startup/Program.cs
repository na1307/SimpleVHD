using SimpleVHD;
using System.Diagnostics;
using static SimpleVHD.BcdEdit;

try {
    string pvDir, vhdDir;

    var drvs = DriveInfo.GetDrives().Where(Extensions.CheckFixed);
    var pvDrvs = drvs.Where(d => File.Exists(d + Path.DirectorySeparatorChar.ToString() + DirName + Path.DirectorySeparatorChar.ToString() + ConfigName)).Select(d => d.Name);
    var vhdDrvs = drvs.Where(d => File.Exists(d + PVConfig.Instance.VhdDirectory + PVConfig.Instance.VhdFile)).Select(Extensions.GetLetter);

    pvDir = pvDrvs.Any() ? pvDrvs.First() + DirName + Path.DirectorySeparatorChar.ToString() : throw new FileNotFoundException("설정 파일을 찾을 수 없습니다.", ConfigName);
    vhdDir = vhdDrvs.Any() ? vhdDrvs.First() + PVConfig.Instance.VhdDirectory : throw new FileNotFoundException("VHD 파일을 찾을 수 없습니다.", PVConfig.Instance.VhdFile);

    if (PVConfig.Instance.Action == DoAction.DoUninstall) {
        uninstall(pvDir, vhdDir);
        return;
    }

    if (PVConfig.Instance.OperatingStyle == OperatingStyle.Simple) return;

    if (PVConfig.Instance.Action == DoAction.DoParentBoot && PVConfig.Instance.VhdFormat == VhdFormat.Vhdx) {
        rebuild(vhdDir);
        return;
    }

    PVConfig.Instance.Action = DoAction.DoNothing;
    PVConfig.Instance.SaveConfig();

    switch (PVConfig.Instance.OperatingStyle) {
        case OperatingStyle.DifferentialManual:
            manual(vhdDir);
            break;

        case OperatingStyle.DifferentialAuto:
            auto(vhdDir);
            break;
    }
} catch (Exception ex) {
    MessageBox.Show(ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
    return;
}

static void copy(string vhdDir, string vhd) => File.Copy(vhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), vhdDir + vhd + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);

static void rebuild(string vhdDir) {
    checkadmin();

    copy(vhdDir, Child1Name);
    copy(vhdDir, Child2Name);

    PVConfig.Instance.Action = DoAction.DoRebuild;
    PVConfig.Instance.SaveConfig();

    ProcessBcdEdit($"/bootsequence {PVConfig.Instance.GetGuid(GuidType.PE)}");

    MessageBox.Show("VHDX 포맷을 사용 중인 상태에서 원본 윈도우로 부팅하였습니다.\r\n\r\n시스템 재시작시 자동으로 자식 VHD를 재구축하도록 작업이 예약되었습니다. 그러니 지금은 초기화와 같은 다른 작업들은 진행하지 마시길 바라며, 원하는 작업을 모두 마친 후 그대로 시스템을 재시작하시길 바랍니다.", "SimpleVHD", MessageBoxButtons.OK, MessageBoxIcon.Information);
}

static void manual(string vhdDir) {
    try {
        copy(vhdDir, Child2Name);
        copy(vhdDir, Child1Name);
    } catch (IOException) {
        //
    }
}

static void auto(string vhdDir) {
    checkadmin();

    var guidc = BcdEditRegex("/enum {current} /v", @"^identifier\s+(?<guid>\{.+\})").Groups["guid"].Value;
    var guid1 = PVConfig.Instance.GetGuid(GuidType.Child1);
    var guid2 = PVConfig.Instance.GetGuid(GuidType.Child2);

    if (guidc == guid2) {
        copy(vhdDir, Child1Name);
        ProcessBcdEdit($"/default {guid1}");
        ProcessBcdEdit($"/displayorder {guid1} /addfirst");
        ProcessBcdEdit($"/displayorder {guid2} /remove");
    } else if (guidc == guid1) {
        copy(vhdDir, Child2Name);
        ProcessBcdEdit($"/default {guid2}");
        ProcessBcdEdit($"/displayorder {guid2} /addfirst");
        ProcessBcdEdit($"/displayorder {guid1} /remove");
    } else {
        copy(vhdDir, Child1Name);
        copy(vhdDir, Child2Name);
    }
}

static void uninstall(string pvDir, string vhdDir) {
    checkadmin();

    File.Delete(pvDir + ConfigName);
    File.Delete(pvDir + "Backup-BCD-01");
    File.Delete(pvDir + "Backup-BCD-02");

    ProcessBcdEdit($"/default {PVConfig.Instance.GetGuid(GuidType.Parent)}");
    ProcessBcdEdit($"/displayorder {PVConfig.Instance.GetGuid(GuidType.Parent)} /addfirst");
    ProcessBcdEdit($"/delete {PVConfig.Instance.GetGuid(GuidType.Child1)} /cleanup");
    ProcessBcdEdit($"/delete {PVConfig.Instance.GetGuid(GuidType.Child2)} /cleanup");
    ProcessBcdEdit($"/delete {PVConfig.Instance.GetGuid(GuidType.PE)} /cleanup");
    ProcessBcdEdit($"/delete {PVConfig.Instance.GetGuid(GuidType.Ramdisk)} /cleanup");

    Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true).DeleteValue("PVStartup");

    File.Delete(vhdDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower());
    File.Delete(vhdDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower());
    File.Delete(vhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower());

    MessageBox.Show("SimpleVHD의 제거를 완료하였습니다.", "SimpleVHD", MessageBoxButtons.OK, MessageBoxIcon.Information);
}

static void checkadmin() {
    try {
        using Process process = new() {
            StartInfo = {
                FileName = "diskpart.exe",
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();
        process.Kill();
    } catch (System.ComponentModel.Win32Exception) {
        using Process process = new() {
            StartInfo = {
                FileName = System.Reflection.Assembly.GetExecutingAssembly().Location,
                Verb = "runas",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden
            }
        };

        process.Start();
        Environment.Exit(0);
    }
}