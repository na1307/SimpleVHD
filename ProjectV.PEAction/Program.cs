using ProjectV;

// It is unfortunate but we have to set it to Unknown first.
Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

try {
    if (string.IsNullOrEmpty(PVDir)) {
        ErrMsg("설정 파일을 찾지 못하였습니다.");
        return;
    }

    if (string.IsNullOrEmpty(BackupDir)) {
        ErrMsg("백업 디렉토리를 찾지 못하였습니다.");
        return;
    }

    if (string.IsNullOrEmpty(VhdDir)) {
        ErrMsg("원본 VHD 파일을 찾지 못하였습니다.");
        return;
    }

    Application.ApplicationExit += Nothing;

    if (PVConfig.Instance.Action != DoAction.DoNothing) {
        ProjectV.PEAction.Actions.ActionFactory.Create(PVConfig.Instance.Action).Run();
        return;
    }
} catch (Exception ex) {
    ErrMsg(ex.ToString());
    return;
}

Application.Run(new ProjectV.PEAction.FormMain());

void Nothing(object sender, EventArgs e) {
    Application.ApplicationExit -= Nothing;
    PVConfig.Instance.Action = DoAction.DoNothing;
    PVConfig.Instance.SaveConfig();
}