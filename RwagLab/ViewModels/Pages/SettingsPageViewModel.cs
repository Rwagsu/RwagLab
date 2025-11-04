using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using RwagLab.Services;
using Serilog;

namespace RwagLab.ViewModels.Pages;

public partial class SettingsPageViewModel : ObservableObject {
    private readonly IThemeService? themeService;

    private readonly SettingsService settingsService;

    public SettingsPageViewModel() {
        themeService = App.Current.ThemeService;
        settingsService = App.GetService<SettingsService>();

        ThemeIndex = settingsService.AppColorTheme switch {
            AppTheme.System => 0,
            AppTheme.Light => 1,
            AppTheme.Dark => 2,
            _ => 0
        };

        settingsService.PropertyChanged += SettingsService_PropertyChanged;
    }

    [ObservableProperty]
    public int ThemeIndex { get; set; }

    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task SetTheme() {
        var selectedTheme = ThemeIndex switch {
            0 => AppTheme.System,
            1 => AppTheme.Light,
            2 => AppTheme.Dark,
            _ => AppTheme.System
        };

        if (themeService != null) {
            settingsService.AppColorTheme = selectedTheme;

            return;
        }

        Log.Error($"{nameof(App.Current.ThemeService)} is null.");
    }

    private void SettingsService_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == nameof(SettingsService.AppColorTheme)) {
            ThemeIndex = settingsService.AppColorTheme switch {
                AppTheme.System => 0,
                AppTheme.Light => 1,
                AppTheme.Dark => 2,
                _ => 0
            };
        }
    }
}
