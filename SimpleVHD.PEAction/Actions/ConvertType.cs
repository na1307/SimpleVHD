namespace SimpleVHD.PEAction.Actions;

internal class ConvertType : Action {
    public ConvertType() {
        Name = "VHD 형식 변환";
        NeedBackup = true;
        AfterRebuild = true;
        AfterRevert = true;
        RemoveTempAfterProcess = true;
    }

    protected override void RunCore() {
        File.Delete(VhdDir + PVConfig.Instance.VhdFile);
        ProcessDiskpart($"create vdisk file \"{VhdDir}{PVConfig.Instance.VhdFile}\" source \"{BackupDir}{PVConfig.Instance.VhdFile}\" type {PVConfig.Instance.Temp}");
        PVConfig.Instance.VhdType = (VhdType)Enum.Parse(typeof(VhdType), PVConfig.Instance.Temp!, false);
    }
}