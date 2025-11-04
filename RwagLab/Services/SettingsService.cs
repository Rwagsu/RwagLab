using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;
using RwagLab.Models.Data;
using RwagLab.Models.Data.ServicesData;
using Windows.Storage;

namespace RwagLab.Services;

public class SettingsService {
    private readonly ConfigService configService;
    private readonly IThemeService? themeService;

    private bool isCanWriteToJson = false;

    // Service
    public SettingsService(ConfigService _configService) {
        configService = _configService;
        themeService = App.Current.ThemeService;
        GetConfigs();
    }

    // Json write
    public SettingsService() {
        configService = App.GetService<ConfigService>();
        themeService = App.Current.ThemeService;
    }

    public AppTheme AppColorTheme { 
        get => SettingsServiceData.appColorTheme;
        set {
            SettingsServiceData.appColorTheme = value;

            // Set theme
            Task.Run(async () => {
                if (themeService != null) {
                    await themeService.SetThemeAsync(AppColorTheme);
                }
            });

            // Invoke event and write to json
            NotifyPropertyChanged(nameof(AppColorTheme));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    // Invoke Event and Save Config
    private void NotifyPropertyChanged(string propertyName) {
        if (isCanWriteToJson) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            Task.Run(async () => {
                await configService.WriteConfigAsync(
                    Path.Combine(AppDataPath.pathsList["ConfigsPath"], "ThemeConfigs.json"),
                    SettingsServiceContext.Default.SettingsService,
                    this
                ).ConfigureAwait(false);
            });
        }
    }

    public void GetConfigs() {
        isCanWriteToJson = false;

        configService.TryReadConfig(Path.Combine(AppDataPath.pathsList["ConfigsPath"], @"ThemeConfigs.json"),
            SettingsServiceContext.Default.SettingsService,
            out _
        );

        isCanWriteToJson = true;
    }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(SettingsService))]
internal partial class SettingsServiceContext : JsonSerializerContext;
