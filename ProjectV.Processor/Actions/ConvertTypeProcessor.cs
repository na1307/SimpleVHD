#nullable enable
namespace ProjectV.Processor.Actions;

internal class ConvertTypeProcessor : ActionProcessor {
    protected sealed override bool NeedBackup => true;
    protected sealed override bool AfterRebuild => true;
    protected sealed override bool AfterRevert => true;
    protected sealed override bool RemoveTempAfterProcess => true;

    public ConvertTypeProcessor() : base("VHD 형식 변환") { }

    protected override void DoProcessCore() {
        File.Delete(VHDDir + PVConfig.Instance.VhdFile);
        ProcessDiskpart($"create vdisk file \"{VHDDir}{PVConfig.Instance.VhdFile}\" source \"{BackupDir}{PVConfig.Instance.VhdFile}\" type {PVConfig.Instance.Temp}");
        PVConfig.Instance.VhdType = (VhdType)Enum.Parse(typeof(VhdType), PVConfig.Instance.Temp, false);
    }
}