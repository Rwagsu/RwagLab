using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using RwagLab.Models.Enums;
using RwagLab.Views.Pages;

namespace RwagLab.ViewModels.Pages;

public partial class MainPageViewModel : ObservableObject {
    private readonly IOptions<AppConfig> configService;

    public MainPageViewModel() {
        configService = App.GetService<IOptions<AppConfig>>();
        Title = configService.Value.ApplicationName ?? "AppName";
    }

    [ObservableProperty]
    public string Title { get; set; }

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
}
