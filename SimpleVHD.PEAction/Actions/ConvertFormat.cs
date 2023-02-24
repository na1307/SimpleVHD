using System.Text.RegularExpressions;

namespace SimpleVHD.PEAction.Actions;

internal class ConvertFormat : Action {
    protected override string Name => "VHD 포맷 변환";

    public ConvertFormat() {
        NeedBackup = true;
        RemoveTempAfterProcess = true;
    }

    protected override void RunCore() {
        // 기존 파일 삭제
        File.Delete(VhdDir + PVConfig.Instance.VhdFile);
        File.Delete(VhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower());
        File.Delete(VhdDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower());
        File.Delete(VhdDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower());

        // 새 형식 지정
        if (!Enum.TryParse(PVConfig.Instance.Temp, false, out VhdFormat newFormat)) throw new InvalidTempException();
        var newVhdRegex = Regex.Match(PVConfig.Instance.VhdFile, @"^(?<filename>.+\.)vhdx?$", RegexOptions.IgnoreCase);
        var newVhd = newVhdRegex.Success ? newVhdRegex.Groups["filename"].Value + newFormat.ToString().ToLower() : throw new ProcessFailedException("정규식 오류");
        var newChildC = ChildCName + newFormat.ToString().ToLower();
        var newChild1 = Child1Name + newFormat.ToString().ToLower();
        var newChild2 = Child2Name + newFormat.ToString().ToLower();

        // 변환
        ProcessDiskpart($"create vdisk file \"{VhdDir}{newVhd}\" source \"{BackupDir}{PVConfig.Instance.VhdFile}\" type {PVConfig.Instance.VhdType}");

        // 자식 생성
        if (IsDifferentialStyle) {
            ProcessDiskpart($"create vdisk file \"{VhdDir}{newChildC}\" parent \"{VhdDir}{newVhd}\"");
            File.Copy(VhdDir + newChildC, VhdDir + newChild1, true);
            File.Copy(VhdDir + newChildC, VhdDir + newChild2, true);
        }

        // BCD 업데이트
        var drv = VhdDir.Left(2);

        ProcessBcdEdit($"/set {PVConfig.Instance.GetGuid(GuidType.Parent)} device vhd=\"[{drv}]{PVConfig.Instance.VhdDirectory}{newVhd}\"", $"/set {PVConfig.Instance.GetGuid(GuidType.Parent)} osdevice vhd=\"[{drv}]{PVConfig.Instance.VhdDirectory}{newVhd}\"");
        ProcessBcdEdit($"/set {PVConfig.Instance.GetGuid(GuidType.Child1)} device vhd=\"[{drv}]{PVConfig.Instance.VhdDirectory}{newChild1}\"", $"/set {PVConfig.Instance.GetGuid(GuidType.Child1)} osdevice vhd=\"[{drv}]{PVConfig.Instance.VhdDirectory}{newChild1}\"");
        ProcessBcdEdit($"/set {PVConfig.Instance.GetGuid(GuidType.Child2)} device vhd=\"[{drv}]{PVConfig.Instance.VhdDirectory}{newChild2}\"", $"/set {PVConfig.Instance.GetGuid(GuidType.Child2)} osdevice vhd=\"[{drv}]{PVConfig.Instance.VhdDirectory}{newChild2}\"");

        // 백업 파일 삭제
        File.Delete(BackupDir + PVConfig.Instance.VhdFile);

        // 백업 파일 변환
        ProcessDiskpart($"create vdisk file \"{BackupDir}{newVhd}\" source \"{VhdDir}{newVhd}\" type expandable");

        PVConfig.Instance.VhdFormat = newFormat;
    }
}