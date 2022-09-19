#nullable enable
namespace ProjectV.Processor.Actions;

internal class MergeProcessor : ActionProcessor {
    protected sealed override bool DifferentialOnly => true;
    protected sealed override bool AfterRebuild => true;
    protected sealed override bool AfterRevert => true;

    public MergeProcessor() : base("병합", Config[ShutdownType.Merge]) { }

    protected override void DoProcessCore() {
        // 파일 크기 측정
        var clean = new FileInfo(VHDDir + ChildCName + Config.VhdFormat.ToString().ToLower()).Length;
        var child1 = new FileInfo(VHDDir + Child1Name + Config.VhdFormat.ToString().ToLower()).Length;
        var child2 = new FileInfo(VHDDir + Child2Name + Config.VhdFormat.ToString().ToLower()).Length;

        string pc;

        if (clean == child1 && clean == child2) {
            throw new ProcessFailedException("병합할 항목이 없습니다.");
        } else if (clean == child1 && clean != child2) {
            pc = Child2Name + Config.VhdFormat.ToString().ToLower();
        } else if (clean != child1 && clean == child2) {
            pc = Child1Name + Config.VhdFormat.ToString().ToLower();
        } else {
            throw new ProcessFailedException("자식 VHD 둘 모두 깨끗한 VHD가 아닙니다. 초기화 시스템이 정상적으로 작동되지 않았을 수 있습니다.");
        }

        ProcessDiskpart($"select vdisk file \"{VHDDir}{pc}\"", "merge vdisk depth 1");
    }
}