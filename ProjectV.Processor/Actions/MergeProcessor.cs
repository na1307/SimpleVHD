#nullable enable
namespace ProjectV.Processor.Actions;

internal class MergeProcessor : ActionProcessor {
    protected sealed override bool DifferentialOnly => true;
    protected sealed override bool AfterRebuild => true;
    protected sealed override bool AfterRevert => true;

    public MergeProcessor() : base("병합", PVConfig.Instance[ShutdownType.Merge]) { }

    protected override void DoProcessCore() {
        // 파일 크기 측정
        var clean = new FileInfo(VhdDir + ChildCName + PVConfig.Instance.VhdFormat.ToString().ToLower()).Length;
        var child1 = new FileInfo(VhdDir + Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower()).Length;
        var child2 = new FileInfo(VhdDir + Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower()).Length;

        string child;

        if (clean == child1 && clean == child2) {
            throw new ProcessFailedException("병합할 항목이 없습니다.");
        } else if (clean == child1 && clean != child2) {
            child = Child2Name + PVConfig.Instance.VhdFormat.ToString().ToLower();
        } else if (clean != child1 && clean == child2) {
            child = Child1Name + PVConfig.Instance.VhdFormat.ToString().ToLower();
        } else {
            throw new ProcessFailedException("자식 VHD 둘 모두 깨끗한 VHD가 아닙니다. 초기화 시스템이 정상적으로 작동되지 않았을 수 있습니다.");
        }

        ProcessDiskpart($"select vdisk file \"{VhdDir}{child}\"", "merge vdisk depth 1");
    }
}