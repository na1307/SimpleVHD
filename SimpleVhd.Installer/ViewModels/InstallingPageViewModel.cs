using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using SimpleVhd.Installer.Models;

namespace SimpleVhd.Installer.ViewModels;

public sealed partial class InstallingPageViewModel : StepPageViewModel {
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CompletedVisiblity))]
    private bool installing = true;

    public override string Title => "설치 중";
    public override string Description => "설치가 진행 중입니다. 잠시만 기다려 주세요... 설치가 완료되면 자동으로 창이 닫힙니다.";
    public override bool CanNext => false;
    public override bool CanBack => false;
    public Visibility CompletedVisiblity => Installing ? Visibility.Collapsed : Visibility.Visible;

    public async Task ProcessAsync() {
        await Task.Run(InstallProcessor.Model!.InstallProcess);
        Installing = false;
        await Task.Delay(1500);
        Application.Current.Exit();
    }
}
