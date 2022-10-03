using ProjectV;
using static ProjectV.BcdEdit;

try {
    string pvDir = string.Empty;
    string vhdDir = string.Empty;

    foreach (var drv in DriveInfo.GetDrives().Where(Extensions.CheckFixed).Select(Extensions.GetLetter)) {
        if (File.Exists(drv + "\\" + DirName + "\\" + ConfigName)) pvDir = drv + "\\" + DirName + "\\";
        if (File.Exists(drv + PVConfig.Instance.VhdDirectory + PVConfig.Instance.VhdFile)) vhdDir = drv + PVConfig.Instance.VhdDirectory;
    }

    if (PVConfig.Instance.Action == DoAction.DoUninstall) {
        uninstall(pvDir, vhdDir);
        return;
    }

    if (PVConfig.Instance.OperatingStyle == OperatingStyle.Simple) return;

    if (PVConfig.Instance.Action == DoAction.DoParentBoot && PVConfig.Instance.VhdFormat == VhdFormat.VHDX) {
        rebuild(vhdDir);
        return;
    }

    PVConfig.Instance.Action = DoAction.DoNothing;

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

static void rebuild(string vhdDir) {
    File.Copy(vhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), vhdDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);
    File.Copy(vhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), vhdDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);

    PVConfig.Instance.Action = DoAction.DoRebuild;
    PVConfig.Instance.SaveConfig();

    ProcessBcdEdit($"/bootsequence {PVConfig.Instance[GuidType.Processor]}");

    MessageBox.Show("VHDX 포맷을 사용 중인 상태에서 원본 윈도우로 부팅하였습니다.\r\n\r\n시스템 재시작시 자동으로 자식 VHD를 재구축하도록 작업이 예약되었습니다. 그러니 지금은 초기화와 같은 다른 작업들은 진행하지 마시길 바라며, 원하는 작업을 모두 마친 후 그대로 시스템을 재시작하시길 바랍니다.", "Project V", MessageBoxButtons.OK, MessageBoxIcon.Information);
}

static void manual(string vhdDir) {
    try {
        File.Copy(vhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), vhdDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);
        File.Copy(vhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), vhdDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);
    } catch (IOException) {
        //
    }
}

static void auto(string vhdDir) {
    string guidc = BcdEditRegex("/enum {current} /v", @"^identifier\s+(?<guid>\{.+\})").Groups["guid"].Value;
    string guid1 = PVConfig.Instance[GuidType.Child1];
    string guid2 = PVConfig.Instance[GuidType.Child2];

    if (guidc == guid2) {
        File.Copy(vhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), vhdDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);
        ProcessBcdEdit($"/default {guid1}");
        ProcessBcdEdit($"/displayorder {guid1} /addfirst");
        ProcessBcdEdit($"/displayorder {guid2} /remove");
    } else if (guidc == guid1) {
        File.Copy(vhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), vhdDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);
        ProcessBcdEdit($"/default {guid2}");
        ProcessBcdEdit($"/displayorder {guid2} /addfirst");
        ProcessBcdEdit($"/displayorder {guid1} /remove");
    } else {
        File.Copy(vhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), vhdDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);
        File.Copy(vhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), vhdDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);
    }
}

static void uninstall(string pvDir, string vhdDir) {
    File.Delete(pvDir + ConfigName);
    File.Delete(pvDir + "Backup-BCD-01");
    File.Delete(pvDir + "Backup-BCD-02");

    ProcessBcdEdit($"/default {PVConfig.Instance[GuidType.Parent]}");
    ProcessBcdEdit($"/displayorder {PVConfig.Instance[GuidType.Parent]} /addfirst");
    ProcessBcdEdit($"/delete {PVConfig.Instance[GuidType.Child1]} /cleanup");
    ProcessBcdEdit($"/delete {PVConfig.Instance[GuidType.Child2]} /cleanup");
    ProcessBcdEdit($"/delete {PVConfig.Instance[GuidType.Processor]} /cleanup");
    ProcessBcdEdit($"/delete {PVConfig.Instance[GuidType.Ramdisk]} /cleanup");

    Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true).DeleteValue("PVStartup");

    File.Delete(vhdDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower());
    File.Delete(vhdDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower());
    File.Delete(vhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower());

    MessageBox.Show("Project V의 제거를 완료하였습니다.", "Project V", MessageBoxButtons.OK, MessageBoxIcon.Information);
}