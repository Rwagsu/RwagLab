using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using RwagLab.Models.Enums;
using RwagLab.Services;
using RwagLab.Views.Pages;
using RwagLab.Views.Pages.Settings;

namespace RwagLab.ViewModels.Pages;

public partial class MainPageViewModel : ObservableObject {
    private readonly AppConfig config;

    private readonly SettingsService settingsService;

    public MainPageViewModel() {
        config = App.Configuration.Value;
        settingsService = App.GetService<SettingsService>();

        Title = config.ApplicationName ?? "AppName";

        SetBackground();

        settingsService.PropertyChanged += SettingsService_PropertyChanged;
    }

    [ObservableProperty]
    public partial string Title { get; set; }

    [ObservableProperty]
    public partial Brush? BackgroundBrush { get; set; }

    [RelayCommand]
    private void NavigateToPage(NavigationViewSelectionChangedEventArgs e) {
        if (e.IsSettingsSelected) {
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<Type>(typeof(SettingsPage)), MessengerTokenEnum.MainPage_PageNavigateToken.ToString());
        }
    }

    [RelayCommand]
    private void NavigateGoBack() {
        WeakReferenceMessenger.Default.Send(new RequestMessage<bool>(), MessengerTokenEnum.MainPage_PageGoBackToken.ToString());
    }

    private async void SetBackground() {
        BackgroundBrush = await settingsService.GetAppBackground();
    }

    private async void SettingsService_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
        switch (e.PropertyName) {
            case nameof(SettingsService.BackgroundType):
                BackgroundBrush = await settingsService.GetAppBackground();
                break;
            case nameof(SettingsService.BackgroundImageStretch):
            case nameof(SettingsService.BackgroundImagePath):
                if (settingsService.BackgroundType != BackgroundTypeEnum.None) {
                    BackgroundBrush = await settingsService.GetAppBackground();
                }
                break;
        }
    }
}
