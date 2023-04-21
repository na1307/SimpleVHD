namespace SimpleVHD.PEAction.Actions;

internal class SwitchStyle : Action {
    protected override string Name => "운영 스타일 전환";

    public SwitchStyle() {
        RemoveTempAfterProcess = true;
    }

    protected override void RunCore() {
        // 파일 삭제
        File.Delete(VhdDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower());
        File.Delete(VhdDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower());
        File.Delete(VhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower());

        if (!Enum.TryParse(PVConfig.Instance.Temp, false, out OperatingStyle operatingStyle)) throw new InvalidTempException();

        switch (operatingStyle) {
            // 단순 스타일
            case OperatingStyle.Simple:
                // BCD 업데이트
                ProcessBcdEdit("/default " + PVConfig.Instance.GetGuid(GuidType.Parent));
                ProcessBcdEdit("/displayorder " + PVConfig.Instance.GetGuid(GuidType.Parent) + " /addfirst");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance.GetGuid(GuidType.PE) + " /addlast");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance.GetGuid(GuidType.Child1) + " /remove");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance.GetGuid(GuidType.Child2) + " /remove");
                break;

            // 차등 스타일 (수동 초기화)
            case OperatingStyle.DifferentialManual:
                // 자식 VHD 재구축
                ProcessDiskpart($"create vdisk file \"{VhdDir}{ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower()}\" parent \"{VhdDir}{PVConfig.Instance.VhdFile}\"");

                File.Copy(VhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), VhdDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);

                // BCD 업데이트
                ProcessBcdEdit("/default " + PVConfig.Instance.GetGuid(GuidType.Child1));
                ProcessBcdEdit("/displayorder " + PVConfig.Instance.GetGuid(GuidType.Child1) + " /addfirst");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance.GetGuid(GuidType.PE) + " /addlast");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance.GetGuid(GuidType.Parent) + " /remove");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance.GetGuid(GuidType.Child2) + " /remove");
                break;

            // 차등 스타일 (자동 초기화)
            case OperatingStyle.DifferentialAuto:
                // 자식 VHD 재구축
                ProcessDiskpart($"create vdisk file \"{VhdDir}{ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower()}\" parent \"{VhdDir}{PVConfig.Instance.VhdFile}\"");

                File.Copy(VhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), VhdDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);
                File.Copy(VhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower(), VhdDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower(), true);

                // BCD 업데이트
                ProcessBcdEdit("/default " + PVConfig.Instance.GetGuid(GuidType.Child1));
                ProcessBcdEdit("/displayorder " + PVConfig.Instance.GetGuid(GuidType.Child1) + " /addfirst");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance.GetGuid(GuidType.PE) + " /addlast");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance.GetGuid(GuidType.Parent) + " /remove");
                ProcessBcdEdit("/displayorder " + PVConfig.Instance.GetGuid(GuidType.Child2) + " /remove");
                break;

            default:
                throw new InvalidTempException();
        }

        PVConfig.Instance.OperatingStyle = operatingStyle;
    }
}