using ProjectV;
using static ProjectV.BcdEdit;

PVConfig config;

string pvDir = string.Empty;
string vhdDir = string.Empty;

try {
    config = PVConfig.Instance;

    foreach (var drv in DriveInfo.GetDrives().Where(Extensions.CheckFixed).Select(Extensions.GetLetter)) {
        if (File.Exists(drv + "\\" + DirName + "\\" + ConfigName)) pvDir = drv + "\\" + DirName + "\\";
        if (File.Exists(drv + config.VhdDirectory + config.VhdFile)) vhdDir = drv + config.VhdDirectory;
    }

    if (config.Action == DoAction.DoUninstall) {
        uninstall();
        return;
    }

    if (config.OperatingStyle == OperatingStyle.Simple) return;

    if (config.Action == DoAction.DoParentBoot && config.VhdFormat == VhdFormat.VHDX) {
        rebuild();
        return;
    }

    config.Action = DoAction.DoNothing;

    switch (config.OperatingStyle) {
        case OperatingStyle.DifferentialManual:
            manual();
            break;

        case OperatingStyle.DifferentialAuto:
            auto();
            break;
    }
} catch (Exception ex) {
    MessageBox.Show(ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
    return;
}

void rebuild() {
    File.Copy(vhdDir + ChildCName + config.VhdFormat.ToString().ToLower(), vhdDir + Child1Name + config.VhdFormat.ToString().ToLower(), true);
    File.Copy(vhdDir + ChildCName + config.VhdFormat.ToString().ToLower(), vhdDir + Child2Name + config.VhdFormat.ToString().ToLower(), true);

    config.Action = DoAction.DoRebuild;
    config.SaveConfig();

    ProcessBcdEdit($"/bootsequence {config[GuidType.Processor]}");

    MessageBox.Show("VHDX 포맷을 사용 중인 상태에서 원본 윈도우로 부팅하였습니다.\r\n\r\n시스템 재시작시 자동으로 자식 VHD를 재구축하도록 작업이 예약되었습니다. 그러니 지금은 초기화와 같은 다른 작업들은 진행하지 마시길 바라며, 원하는 작업을 모두 마친 후 그대로 시스템을 재시작하시길 바랍니다.", "Project V", MessageBoxButtons.OK, MessageBoxIcon.Information);
}

void manual() {
    try {
        File.Copy(vhdDir + ChildCName + config.VhdFormat.ToString().ToLower(), vhdDir + Child2Name + config.VhdFormat.ToString().ToLower(), true);
        File.Copy(vhdDir + ChildCName + config.VhdFormat.ToString().ToLower(), vhdDir + Child1Name + config.VhdFormat.ToString().ToLower(), true);
    } catch (IOException) {
        //
    }
}

void auto() {
    string guidc = BcdEditRegex("/enum {current} /v", @"^identifier\s+(?<guid>\{.+\})").Groups["guid"].Value;
    string guid1 = config[GuidType.Child1];
    string guid2 = config[GuidType.Child2];

    if (guidc == guid2) {
        File.Copy(vhdDir + ChildCName + config.VhdFormat.ToString().ToLower(), vhdDir + Child1Name + config.VhdFormat.ToString().ToLower(), true);
        ProcessBcdEdit($"/default {guid1}");
        ProcessBcdEdit($"/displayorder {guid1} /addfirst");
        ProcessBcdEdit($"/displayorder {guid2} /remove");
    } else if (guidc == guid1) {
        File.Copy(vhdDir + ChildCName + config.VhdFormat.ToString().ToLower(), vhdDir + Child2Name + config.VhdFormat.ToString().ToLower(), true);
        ProcessBcdEdit($"/default {guid2}");
        ProcessBcdEdit($"/displayorder {guid2} /addfirst");
        ProcessBcdEdit($"/displayorder {guid1} /remove");
    } else {
        File.Copy(vhdDir + ChildCName + config.VhdFormat.ToString().ToLower(), vhdDir + Child1Name + config.VhdFormat.ToString().ToLower(), true);
        File.Copy(vhdDir + ChildCName + config.VhdFormat.ToString().ToLower(), vhdDir + Child2Name + config.VhdFormat.ToString().ToLower(), true);
    }
}

void uninstall() {
    File.Delete(pvDir + ConfigName);
    File.Delete(pvDir + "Backup-BCD-01");
    File.Delete(pvDir + "Backup-BCD-02");

    ProcessBcdEdit($"/default {config[GuidType.Parent]}");
    ProcessBcdEdit($"/displayorder {config[GuidType.Parent]} /addfirst");
    ProcessBcdEdit($"/delete {config[GuidType.Child1]} /cleanup");
    ProcessBcdEdit($"/delete {config[GuidType.Child2]} /cleanup");
    ProcessBcdEdit($"/delete {config[GuidType.Processor]} /cleanup");
    ProcessBcdEdit($"/delete {config[GuidType.Ramdisk]} /cleanup");

    Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true).DeleteValue("PV_Agent");
    File.Delete(pvDir + "Agent.vbs");

    File.Delete(vhdDir + Child1Name + config.VhdFormat.ToString().ToLower());
    File.Delete(vhdDir + Child2Name + config.VhdFormat.ToString().ToLower());
    File.Delete(vhdDir + ChildCName + config.VhdFormat.ToString().ToLower());

    MessageBox.Show("Project V의 제거를 완료하였습니다.", "Project V", MessageBoxButtons.OK, MessageBoxIcon.Information);
}