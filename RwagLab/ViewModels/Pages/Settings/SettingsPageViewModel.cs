using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using RwagLab.Models.Enums;
using RwagLab.Services;
using RwagLab.Views.Pages.Settings;
using Serilog;

namespace RwagLab.ViewModels.Pages.Settings;

public partial class SettingsPageViewModel : ObservableObject {


    public SettingsPageViewModel() {
        
    }

    [RelayCommand]
    private void NavigateToPage(NavigationViewSelectionChangedEventArgs e) {
        var selectedItem = (NavigationViewItem)e.SelectedItem;

        switch (selectedItem.Tag.ToString()) {
            case "AboutSettings":
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<Type>(typeof(AboutSettingsPage)), MessengerTokenEnum.SettingsPage_PageNavigateToken.ToString());
                break;

            case "ThemeSettings":
                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<Type>(typeof(ThemeSettingsPage)), MessengerTokenEnum.SettingsPage_PageNavigateToken.ToString());
                break;
        }
    }
}
