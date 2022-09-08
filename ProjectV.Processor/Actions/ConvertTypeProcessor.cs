#nullable enable
namespace ProjectV.Processor.Actions;

internal class ConvertTypeProcessor : ActionProcessor {
    protected sealed override bool NeedBackup => true;
    protected sealed override bool AfterRebuild => true;
    protected sealed override bool AfterRevert => true;
    protected sealed override bool RemoveTempAfterProcess => true;

    public ConvertTypeProcessor() : base("VHD 형식 변환") { }

    protected override void DoProcessCore() {
        File.Delete(VHDDir + VF);
        ProcessDiskpart($"create vdisk file \"{VHDDir}{VF}\" source \"{BackupDir}{VF}\" type {Config.Temp}");
        Config.VhdType = (VhdType)Enum.Parse(typeof(VhdType), Config.Temp, false);
    }
}