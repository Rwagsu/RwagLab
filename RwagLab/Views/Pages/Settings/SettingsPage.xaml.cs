using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using RwagLab.Models.Enums;
using RwagLab.ViewModels.Pages;
using RwagLab.ViewModels.Pages.Settings;
using Uno.UI.RemoteControl;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static System.Net.WebRequestMethods;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RwagLab.Views.Pages.Settings;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsPage : Page {
    public SettingsPage() {
        this.InitializeComponent();
        DataContext = new SettingsPageViewModel();

        SettingsNavigationView.SelectedItem = SettingsNavigationView.MenuItems[0];
        SettingsFrame.Navigate(typeof(AboutSettingsPage));

        //Register NavigationToPage Messengers
        WeakReferenceMessenger.Default.Register<ValueChangedMessage<Type>, string>(this, MessengerTokenEnum.SettingsPage_PageNavigateToken.ToString(), NavigateToPage);
    }

    private void NavigateToPage(object recipient, ValueChangedMessage<Type> message) {
        if (SettingsFrame.Content != null && SettingsFrame.Content.GetType() == message.Value && !typeof(Page).IsAssignableFrom(message.Value)) {
            return;
        }

        SettingsFrame.Navigate(message.Value);
    }
}
