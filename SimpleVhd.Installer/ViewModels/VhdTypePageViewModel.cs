using CommunityToolkit.Mvvm.ComponentModel;
using SimpleVhd.Installer.Models;

namespace SimpleVhd.Installer.ViewModels;

public sealed partial class VhdTypePageViewModel : StepPageViewModel {
    [ObservableProperty]
    private VhdType vhdType;

    public override string Title => "VHD 형식";
    public override string Description => "이 VHD의 형식을 선택해주세요.";
    public override bool CanNext => true;
    public override bool CanBack => true;

    partial void OnVhdTypeChanged(VhdType oldValue, VhdType newValue) {
        if (newValue != oldValue) {
            InstallProcessor.Model!.VhdType = newValue;
        }
    }
}
