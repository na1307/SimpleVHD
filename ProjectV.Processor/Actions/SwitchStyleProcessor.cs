#nullable enable
namespace ProjectV.Processor.Actions;

internal class SwitchStyleProcessor : ActionProcessor {
    protected sealed override bool RemoveTempAfterProcess => true;

    public SwitchStyleProcessor() : base("운영 스타일 전환") { }

    protected override void DoProcessCore() {
        // 파일 삭제
        File.Delete(VHDDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower());
        File.Delete(VHDDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower());
        File.Delete(VHDDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower());

        if (!Enum.TryParse(PVConfig.Instance.Temp, false, out OperatingStyle operatingStyle)) throw new ProcessFailedException("Temp가 잘못되었습니다.");

        switch (operatingStyle) {
            // 단순 스타일
            case OperatingStyle.Simple:
                // BCD 업데이트
                ProcessBcdEdit("/default " + PVConfig.Instance[GuidType.Parent]);
                ProcessBcdEdit("/displayorder " + PVConfig.Instance[GuidType.Parent] + " /addfirst");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance[GuidType.Processor] + " /addlast");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance[GuidType.Child1] + " /remove");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance[GuidType.Child2] + " /remove");
                break;

            // 차등 스타일 (수동 초기화)
            case OperatingStyle.DifferentialManual:
                // 자식 VHD 재구축
                ProcessDiskpart($"create vdisk file \"{VHDDir}{ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower()}\" parent \"{VHDDir}{PVConfig.Instance.VhdFile}\"");

                File.Copy(VHDDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), VHDDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);
                File.Copy(VHDDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), VHDDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);

                // BCD 업데이트
                ProcessBcdEdit("/default " + PVConfig.Instance[GuidType.Child1]);
                ProcessBcdEdit("/displayorder " + PVConfig.Instance[GuidType.Child1] + " /addfirst");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance[GuidType.Processor] + " /addlast");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance[GuidType.Parent] + " /remove");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance[GuidType.Child2] + " /remove");
                break;

            // 차등 스타일 (자동 초기화)
            case OperatingStyle.DifferentialAuto:
                // 자식 VHD 재구축
                ProcessDiskpart($"create vdisk file \"{VHDDir}{ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower()}\" parent \"{VHDDir}{PVConfig.Instance.VhdFile}\"");

                File.Copy(VHDDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), VHDDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);
                File.Copy(VHDDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), VHDDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);

                // BCD 업데이트
                ProcessBcdEdit("/default " + PVConfig.Instance[GuidType.Child1]);
                ProcessBcdEdit("/displayorder " + PVConfig.Instance[GuidType.Child1] + " /addfirst");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance[GuidType.Processor] + " /addlast");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance[GuidType.Parent] + " /remove");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance[GuidType.Child2] + " /remove");
                break;

            default:
                throw new PVConfig.InvalidConfigException("Temp가 잘못되었습니다.");
        }

        PVConfig.Instance.OperatingStyle = operatingStyle;
    }
}