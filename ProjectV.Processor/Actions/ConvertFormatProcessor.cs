#nullable enable
using System.Text.RegularExpressions;

namespace ProjectV.Processor.Actions;

internal class ConvertFormatProcessor : ActionProcessor {
    protected sealed override bool NeedBackup => true;
    protected sealed override bool RemoveTempAfterProcess => true;

    public ConvertFormatProcessor() : base("VHD 포맷 변환") { }

    protected override void DoProcessCore() {
        // 기존 파일 삭제
        File.Delete(VHDDir + PVConfig.Instance.VhdFile);
        File.Delete(VHDDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower());
        File.Delete(VHDDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower());
        File.Delete(VHDDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower());

        // 새 형식 지정
        var newFormat = (VhdFormat)Enum.Parse(typeof(VhdFormat), PVConfig.Instance.Temp, false);
        var newVhdRegex = Regex.Match(PVConfig.Instance.VhdFile, @"^(?<filename>.+\.)vhdx?$", RegexOptions.IgnoreCase);
        var newVhd = newVhdRegex.Success ? newVhdRegex.Groups["filename"].Value + newFormat.ToString().ToLower() : throw new ProcessFailedException("정규식 오류");
        var newChildC = "Clean." + newFormat.ToString().ToLower();
        var newChild1 = "Child1." + newFormat.ToString().ToLower();
        var newChild2 = "Child2." + newFormat.ToString().ToLower();

        // 변환
        ProcessDiskpart($"create vdisk file \"{VHDDir}{newVhd}\" source \"{BackupDir}{PVConfig.Instance.VhdFile}\" type {PVConfig.Instance.VhdType}");

        // 자식 생성
        if (PVConfig.Instance.OperatingStyle is OperatingStyle.DifferentialManual or OperatingStyle.DifferentialAuto) {
            ProcessDiskpart($"create vdisk file \"{VHDDir}{newChildC}\" parent \"{VHDDir}{newVhd}\"");
            File.Copy(VHDDir + newChildC, VHDDir + newChild1, true);
            File.Copy(VHDDir + newChildC, VHDDir + newChild2, true);
        }

        // BCD 업데이트
        ProcessBcdEdit($"/set {PVConfig.Instance[GuidType.Parent]} device vhd=\"[{VHDDrv}]{PVConfig.Instance.VhdDirectory}{newVhd}\"", $"/set {PVConfig.Instance[GuidType.Parent]} osdevice vhd=\"[{VHDDrv}]{PVConfig.Instance.VhdDirectory}{newVhd}\"");
        ProcessBcdEdit($"/set {PVConfig.Instance[GuidType.Child1]} device vhd=\"[{VHDDrv}]{PVConfig.Instance.VhdDirectory}{newChild1}\"", $"/set {PVConfig.Instance[GuidType.Child1]} osdevice vhd=\"[{VHDDrv}]{PVConfig.Instance.VhdDirectory}{newChild1}\"");
        ProcessBcdEdit($"/set {PVConfig.Instance[GuidType.Child2]} device vhd=\"[{VHDDrv}]{PVConfig.Instance.VhdDirectory}{newChild2}\"", $"/set {PVConfig.Instance[GuidType.Child2]} osdevice vhd=\"[{VHDDrv}]{PVConfig.Instance.VhdDirectory}{newChild2}\"");

        // 백업 파일 삭제
        File.Delete(BackupDrv + PVConfig.Instance.VhdFile);

        // 백업 파일 변환
        ProcessDiskpart($"create vdisk file \"{BackupDir}{newVhd}\" source \"{VHDDir}{newVhd}\" type expandable");

        PVConfig.Instance.VhdFormat = newFormat;
    }
}