#nullable enable
namespace ProjectV.Processor.Actions;

internal class SwitchStyleProcessor : ActionProcessor {
    protected sealed override bool RemoveTempAfterProcess => true;

    public SwitchStyleProcessor() : base("운영 스타일 전환") { }

    protected override void DoProcessCore() {
        // 파일 삭제
        File.Delete(VHDDir + Child1Name + Config.VhdFormat.ToString().ToLower());
        File.Delete(VHDDir + Child2Name + Config.VhdFormat.ToString().ToLower());
        File.Delete(VHDDir + ChildCName + Config.VhdFormat.ToString().ToLower());

        var operatingStyle = (OperatingStyle)Enum.Parse(typeof(OperatingStyle), Config.Temp, false);

        switch (operatingStyle) {
            // 단순 스타일
            case OperatingStyle.Simple:
                // BCD 업데이트
                ProcessBcdEdit("/default " + Config[GuidType.Parent]);
                ProcessBcdEdit("/displayorder " + Config[GuidType.Parent] + " /addfirst");
                ProcessBcdEdit("/displayorder " + Config[GuidType.Processor] + " /addlast");
                ProcessBcdEdit("/displayorder " + Config[GuidType.Child1] + " /remove");
                ProcessBcdEdit("/displayorder " + Config[GuidType.Child2] + " /remove");
                break;

            // 차등 스타일 (수동 초기화)
            case OperatingStyle.DifferentialManual:
                // 자식 VHD 재구축
                ProcessDiskpart($"create vdisk file \"{VHDDir}{ChildCName + Config.VhdFormat.ToString().ToLower()}\" parent \"{VHDDir}{VF}\"");

                File.Copy(VHDDir + ChildCName + Config.VhdFormat.ToString().ToLower(), VHDDir + Child1Name + Config.VhdFormat.ToString().ToLower(), true);
                File.Copy(VHDDir + ChildCName + Config.VhdFormat.ToString().ToLower(), VHDDir + Child2Name + Config.VhdFormat.ToString().ToLower(), true);

                // BCD 업데이트
                ProcessBcdEdit("/default " + Config[GuidType.Child1]);
                ProcessBcdEdit("/displayorder " + Config[GuidType.Child1] + " /addfirst");
                ProcessBcdEdit("/displayorder " + Config[GuidType.Processor] + " /addlast");
                ProcessBcdEdit("/displayorder " + Config[GuidType.Parent] + " /remove");
                ProcessBcdEdit("/displayorder " + Config[GuidType.Child2] + " /remove");
                break;

            // 차등 스타일 (자동 초기화)
            case OperatingStyle.DifferentialAuto:
                // 자식 VHD 재구축
                ProcessDiskpart($"create vdisk file \"{VHDDir}{ChildCName + Config.VhdFormat.ToString().ToLower()}\" parent \"{VHDDir}{VF}\"");

                File.Copy(VHDDir + ChildCName + Config.VhdFormat.ToString().ToLower(), VHDDir + Child1Name + Config.VhdFormat.ToString().ToLower(), true);
                File.Copy(VHDDir + ChildCName + Config.VhdFormat.ToString().ToLower(), VHDDir + Child2Name + Config.VhdFormat.ToString().ToLower(), true);

                // BCD 업데이트
                ProcessBcdEdit("/default " + Config[GuidType.Child1]);
                ProcessBcdEdit("/displayorder " + Config[GuidType.Child1] + " /addfirst");
                ProcessBcdEdit("/displayorder " + Config[GuidType.Processor] + " /addlast");
                ProcessBcdEdit("/displayorder " + Config[GuidType.Parent] + " /remove");
                ProcessBcdEdit("/displayorder " + Config[GuidType.Child2] + " /remove");
                break;

            default:
                throw new PVConfig.InvalidConfigException("Temp가 잘못되었습니다.");
        }

        Config.OperatingStyle = operatingStyle;
    }
}