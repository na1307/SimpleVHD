﻿using SimpleVhd.ControlPanel.ViewModels;

namespace SimpleVhd.ControlPanel.Views;

public sealed class RestoreScreen(IMainWindow window, IScreen previousScreen) : OperationScreen {
    protected override RestoreScreenViewModel ViewModel { get; } = new(window, previousScreen);
}
