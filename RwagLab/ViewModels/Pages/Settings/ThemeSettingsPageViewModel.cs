using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using RwagLab.Models.Enums;
using RwagLab.Services;
using Serilog;
using Windows.Storage.Pickers;

namespace RwagLab.ViewModels.Pages.Settings;

public partial class ThemeSettingsPageViewModel : ObservableObject {
    private readonly IThemeService? themeService;
    private readonly ResourceLoader resourceLoader;
    private readonly PathService pathService;

    private readonly SettingsService settingsService;

    public ThemeSettingsPageViewModel() {
        themeService = App.Current.ThemeService;
        settingsService = App.GetService<SettingsService>();
        resourceLoader = App.GetService<ResourceLoader>();
        pathService = App.GetService<PathService>();

        ThemeIndex = settingsService.AppColorTheme switch {
            AppTheme.System => 0,
            AppTheme.Light => 1,
            AppTheme.Dark => 2,
            _ => 0
        };

        BackgroundIndex = settingsService.BackgroundType switch {
            BackgroundTypeEnum.Image => 1,
            BackgroundTypeEnum.BingWallpaper => 2,
            _ => 0
        };

        BackgroundPath = settingsService.BackgroundImagePath;

        BackgroundPathErrorTip = string.Empty;

        settingsService.PropertyChanged += SettingsService_PropertyChanged;
    }

    [ObservableProperty]
    public partial int ThemeIndex { get; set; }

    [ObservableProperty]
    public partial int BackgroundIndex { get; set; }

    [ObservableProperty]
    public partial string BackgroundPath { get; set; }

    [ObservableProperty]
    public partial string BackgroundPathErrorTip { get; set; }

    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task SetTheme() {
        var selectedTheme = ThemeIndex switch {
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

    [RelayCommand]
    private void SetBackground() {
        settingsService.BackgroundType = BackgroundIndex switch {
            1 => BackgroundTypeEnum.Image,
            2 => BackgroundTypeEnum.BingWallpaper,
            _ => BackgroundTypeEnum.None,
        };
    }

    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task SelectBackground() {
        // Init
        var picker = new FileOpenPicker();

        // Get Window Handle
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Current.MainWindow);
        // Associate the HWND with the file picker
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        // Add filter
        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".png");

        // Button title
        picker.CommitButtonText = resourceLoader.GetString("ThemeSettingsPageViewModel_FilePickerFinishButtonText");

        // Start path
        picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

        // View mode
        picker.ViewMode = PickerViewMode.Thumbnail;

        // Show the picker dialog window
        var file = await picker.PickSingleFileAsync();

        if (file != null) {
            if (!pathService.CheckPath(file.Path)) {
                BackgroundPathErrorTip = resourceLoader.GetString("ThemeSettingsPageViewModel_InvalidPathTipText");
            }
            else if(!File.Exists(file.Path)) {
                BackgroundPathErrorTip = resourceLoader.GetString("ThemeSettingsPageViewModel_FileNotFoundTipText");
            }
            else {
                // Clear error
                BackgroundPathErrorTip = string.Empty;
                settingsService.BackgroundImagePath = file.Path;
            }
        }
    }

    partial void OnBackgroundPathChanged(string value) {
        // Error path
        if (!pathService.CheckPath(BackgroundPath)) {
            BackgroundPathErrorTip = resourceLoader.GetString("ThemeSettingsPageViewModel_InvalidPathTipText");
        }
        else if(!File.Exists(BackgroundPath)) {
            BackgroundPathErrorTip = resourceLoader.GetString("ThemeSettingsPageViewModel_FileNotFoundTipText");
        }

        // Valid path
        else {
            // Clear error
            BackgroundPathErrorTip = string.Empty;

            // Set background path
            settingsService.BackgroundImagePath = BackgroundPath;
        }
    }

    private void SettingsService_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
        switch (e.PropertyName) {
            case nameof(SettingsService.AppColorTheme):
                ThemeIndex = settingsService.AppColorTheme switch {
                    AppTheme.Light => 1,
                    AppTheme.Dark => 2,
                    _ => 0
                };
                break;

            case nameof(SettingsService.BackgroundType):
                BackgroundIndex = settingsService.BackgroundType switch {
                    BackgroundTypeEnum.Image => 1,
                    BackgroundTypeEnum.BingWallpaper => 2,
                    _ => 0
                };
                break;

            case nameof(SettingsService.BackgroundImagePath):
                BackgroundPath = settingsService.BackgroundImagePath;
                break;
        }
    }
}
