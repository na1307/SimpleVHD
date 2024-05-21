﻿using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimpleVhd.ControlPanel.Installer;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class VhdTypePage {
    private readonly InstallInput input;

    public VhdTypePage(InstallInput input) {
        InitializeComponent();
        this.input = input;
    }

    public override string Title => "VHD 형식";
    public override string Description => "이 VHD의 형식을 선택해주세요.";

    private void VhdTypeRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e) => input.VhdType = (VhdType)((RadioButtons)sender).SelectedIndex;
}
