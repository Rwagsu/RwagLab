using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media.Imaging;
using RwagLab.Models.Data;
using RwagLab.Models.Data.ServicesData;
using RwagLab.Models.Enums;
using Windows.Storage;

namespace RwagLab.Services;

public class SettingsService {
    private readonly PathService pathService;
    private readonly IThemeService? themeService;

    private bool isCanWriteToJson = false;

    // Service
    public SettingsService(PathService _pathService) {
        pathService = _pathService;
        themeService = App.Current.ThemeService;
        GetConfigs();
    }

    // Json write
    public SettingsService() {
        pathService = App.GetService<PathService>();
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

    public BackgroundTypeEnum BackgroundType {
        get => SettingsServiceData.backgroundType;
        set {
            SettingsServiceData.backgroundType = value;

            // Invoke event and write to json
            NotifyPropertyChanged(nameof(BackgroundType));
        }
    }

    public string BackgroundImagePath {
        get => SettingsServiceData.backgroundImagePath;
        set {
            SettingsServiceData.backgroundImagePath = value;

            // Invoke event and write to json
            NotifyPropertyChanged(nameof(BackgroundImagePath));
        }
    }

    public Stretch BackgroundImageStretch {
        get => SettingsServiceData.backgroundImageStretch;
        set {
            SettingsServiceData.backgroundImageStretch = value;

            // Invoke event and write to json
            NotifyPropertyChanged(nameof(BackgroundImageStretch));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task<Brush> GetAppBackground() {
        switch (BackgroundType) {
            case BackgroundTypeEnum.Image:
                if (pathService.CheckPath(BackgroundImagePath) && File.Exists(BackgroundImagePath)) {
                    return new ImageBrush() { 
                        ImageSource = new BitmapImage(new Uri(BackgroundImagePath)) 
                    };
                }

                // TODO: TOAST (包含路径文档)
                // TODO: 返回好图(雾)
                return new SolidColorBrush("#0077FF".ToColor());

            case BackgroundTypeEnum.BingWallpaper:
                var bingWallpaperPath = await GetBingWallpaperUrl();
                if (!await pathService.IsValidImageUrl(bingWallpaperPath)) {
                    // TODO: TOAST

                    // TODO: 返回好图(雾)
                    return new SolidColorBrush("#0077FF".ToColor());
                }

                return new ImageBrush() {
                    ImageSource = new BitmapImage(new Uri(bingWallpaperPath)),
                    Stretch = SettingsServiceData.backgroundImageStretch
                };
        }

        var resource = App.Current.Resources["ApplicationPageBackgroundThemeBrush"];
        if (resource is Brush brush) {
            return brush;
        }

        return new SolidColorBrush(Colors.White);
    }

    public async Task<string> GetBingWallpaperUrl() {
        try {
            // Get text
            var jsonText = await App.GetService<HttpClient>().GetStringAsync(App.Configuration.Value.BingWallpaperUrl);

            // To json document
            using JsonDocument doc = JsonDocument.Parse(jsonText);

            // Get value
            var status = doc.RootElement.TryGetProperty("images", out var imagesJsonElement);
            status = imagesJsonElement[0].TryGetProperty("url", out var urlJsonElement);

            // Check status
            if (status == true) {

                // Return
                var url = urlJsonElement.GetString();
                if (url != null) {
                    return "https://cn.bing.com" + url;
                }
            }
            // TODO: TOAST
        }
        catch (Exception ex) {
            switch (ex) {
                case HttpRequestException:
                    // TODO: TOAST
                    break;
                case JsonException:
                    // TODO: TOAST
                    break;
                case SocketException:
                    // TODO: 远程主机关闭连接
                case IOException:
                    // TODO: 传输连接问题
                case OperationCanceledException:
                    // TODO: 超时
                    break;
                default:
                    // TODO: 未知错误
                    throw;
            }
        }
        return string.Empty;
    }

    public void GetConfigs() {
        isCanWriteToJson = false;

        pathService.TryReadConfig(Path.Combine(AppDataPath.pathsList["ConfigsPath"], @"SettingsConfigs.json"),
            SettingsServiceContext.Default.SettingsService,
            out var returnValue
        );

        isCanWriteToJson = true;
    }

    // Invoke Event and Save Config
    private void NotifyPropertyChanged(string propertyName) {
        if (isCanWriteToJson) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            Task.Run(async () => {
                await pathService.WriteConfigAsync(
                    Path.Combine(AppDataPath.pathsList["ConfigsPath"], "SettingsConfigs.json"),
                    SettingsServiceContext.Default.SettingsService,
                    this
                ).ConfigureAwait(false);
            });
        }
    }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(SettingsService))]
internal partial class SettingsServiceContext : JsonSerializerContext;
