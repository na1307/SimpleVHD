#nullable enable
global using System;
global using System.Diagnostics;
global using System.IO;
global using System.Windows.Forms;
global using static ProjectV.BcdEdit;
global using static ProjectV.GlobalConstants;
using ProjectV;
using System.Threading;

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

    Application.ApplicationExit += (s, e) => {
        PVConfig.Instance.Action = DoAction.DoNothing;
        PVConfig.Instance.SaveConfig();
    };

    if (PVConfig.Instance.Action != DoAction.DoNothing) {
        ProjectV.Processor.Actions.ProcessorFactory.Create(PVConfig.Instance.Action).DoProcess();
        return;
    }
} catch (Exception ex) {
    ErrMsg(ex.ToString());
    return;
}

Application.Run(new ProjectV.Processor.FormMain());