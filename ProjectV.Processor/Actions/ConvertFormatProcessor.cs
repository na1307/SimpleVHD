#nullable enable
using System.Text.RegularExpressions;

namespace ProjectV.Processor.Actions;

internal class ConvertFormatProcessor : ActionProcessor {
    protected override bool NeedBackup => true;
    protected override bool RemoveTempAfterProcess => true;

    public ConvertFormatProcessor() : base("VHD 포맷 변환") { }

    protected override void DoProcessCore() {
        // 기존 파일 삭제
        File.Delete(VHDDir + VF);
        File.Delete(VHDDir + ChildCName + Config.VhdFormat.ToString().ToLower());
        File.Delete(VHDDir + Child1Name + Config.VhdFormat.ToString().ToLower());
        File.Delete(VHDDir + Child2Name + Config.VhdFormat.ToString().ToLower());

        // 새 형식 지정
        var newFormat = (VhdFormat)Enum.Parse(typeof(VhdFormat), Config.Temp, false);
        var newVhdRegex = Regex.Match(VF, @"^(?<filename>.+\.)vhdx?$", RegexOptions.IgnoreCase);
        var newVhd = newVhdRegex.Success ? newVhdRegex.Groups["filename"].Value + newFormat.ToString().ToLower() : throw new ProcessFailedException("정규식 오류");
        var newChildC = "Clean." + newFormat.ToString().ToLower();
        var newChild1 = "Child1." + newFormat.ToString().ToLower();
        var newChild2 = "Child2." + newFormat.ToString().ToLower();

        // 변환
        ProcessDiskpart($"create vdisk file \"{VHDDir}{newVhd}\" source \"{BackupDir}{VF}\" type {Config.VhdType}");

        // 자식 생성
        if (Config.OperatingStyle is OperatingStyle.DifferentialManual or OperatingStyle.DifferentialAuto) {
            ProcessDiskpart($"create vdisk file \"{VHDDir}{newChildC}\" parent \"{VHDDir}{newVhd}\"");
            File.Copy(VHDDir + newChildC, VHDDir + newChild1, true);
            File.Copy(VHDDir + newChildC, VHDDir + newChild2, true);
        }

        // BCD 업데이트
        ProcessBcdEdit($"/set {Config[GuidType.Parent]} device vhd=\"[{VHDDrv}]{Config.VhdDirectory}{newVhd}\"", $"/set {Config[GuidType.Parent]} osdevice vhd=\"[{VHDDrv}]{Config.VhdDirectory}{newVhd}\"");
        ProcessBcdEdit($"/set {Config[GuidType.Child1]} device vhd=\"[{VHDDrv}]{Config.VhdDirectory}{newChild1}\"", $"/set {Config[GuidType.Child1]} osdevice vhd=\"[{VHDDrv}]{Config.VhdDirectory}{newChild1}\"");
        ProcessBcdEdit($"/set {Config[GuidType.Child2]} device vhd=\"[{VHDDrv}]{Config.VhdDirectory}{newChild2}\"", $"/set {Config[GuidType.Child2]} osdevice vhd=\"[{VHDDrv}]{Config.VhdDirectory}{newChild2}\"");

        // 백업 파일 삭제
        File.Delete(BackupDrv + VF);

        // 백업 파일 변환
        ProcessDiskpart($"create vdisk file \"{BackupDir}{newVhd}\" source \"{VHDDir}{newVhd}\" type expandable");

        Config.VhdFormat = newFormat;
    }
}