using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using RwagLab.ViewModels.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static System.Net.WebRequestMethods;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RwagLab.Views.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsPage : Page {
    public SettingsPage() {
        this.InitializeComponent();
        DataContext = new SettingsPageViewModel();
    }

    private string GetApplicationName() {
        return App.Current.Configuration?.Value.ApplicationName ?? string.Empty;
    }

    private string GetOwner() {
        return App.Current.Configuration?.Value.Owner ?? string.Empty;
    }

    private Uri GetOwnerLink() {
        return new Uri(App.Current.Configuration?.Value.OwnerLink ?? string.Empty);
    }
}
