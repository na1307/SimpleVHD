namespace ProjectV.PEAction.Actions;

internal class ConvertType : Action {
    protected override string Name => "VHD 형식 변환";
    protected sealed override bool NeedBackup => true;
    protected sealed override bool AfterRebuild => true;
    protected sealed override bool AfterRevert => true;
    protected sealed override bool RemoveTempAfterProcess => true;

    protected override void RunCore() {
        File.Delete(VhdDir + PVConfig.Instance.VhdFile);
        ProcessDiskpart($"create vdisk file \"{VhdDir}{PVConfig.Instance.VhdFile}\" source \"{BackupDir}{PVConfig.Instance.VhdFile}\" type {PVConfig.Instance.Temp}");
        PVConfig.Instance.VhdType = (VhdType)Enum.Parse(typeof(VhdType), PVConfig.Instance.Temp, false);
    }
}