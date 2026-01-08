using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml.Media.Animation;
using RwagLab.Extensions;
using RwagLab.Models.Classes;
using RwagLab.Models.Enums;
using RwagLab.ViewModels.Pages;

namespace RwagLab.Views.Pages;

public sealed partial class MainPage : Page {
    private MainPageViewModel viewModel;
    public MainPage() {
        this.InitializeComponent();

        viewModel = new MainPageViewModel();

        DataContext = viewModel;

        //AddToNavItems(viewModel.NavBarItems);

        //Register NavigationToPage Messengers
        WeakReferenceMessenger.Default.Register<ValueChangedMessage<Type>, string>(this, MessengerTokenEnum.MainPage_PageNavigateToken.ToString(), MainFrameNavigate);
        WeakReferenceMessenger.Default.Register<RequestMessage<bool>, string>(this, MessengerTokenEnum.MainPage_PageGoBackToken.ToString(), MainFrameGoBack);
    }

    private void MainFrameGoBack(object recipient, RequestMessage<bool> message) {
        if (MainFrame.CanGoBack) {
            MainFrame.GoBack();
            message.Reply(true);
        }
        message.Reply(false);
    }

    private void MainFrameNavigate(object recipient, ValueChangedMessage<Type> message) {
        if (MainFrame.Content != null && MainFrame.Content.GetType() == message.Value && !typeof(Page).IsAssignableFrom(message.Value)) {
            return;
        }

        MainFrame.Navigate(message.Value, null, new EntranceNavigationTransitionInfo());
    }

#region NavigationView Item Converter
    public void AddToNavItems(ReadOnlyDictionary<ScriptGroup, List<LabScriptItem>> items) {
        // Unpack the input value
        if (!items.Empty()) {
            // Check for empty collection
            List<NavigationViewItem> SupportSystemGroup = new List<NavigationViewItem>();
            List<NavigationViewItem> NotSupportSystemGroup = new List<NavigationViewItem>();

            // Process each group and its items
            foreach (var group in items) {
                var viewItem = CreateNavItem(group.Key, group.Value);

                SupportSystemGroup.AddIfNotNull(viewItem.SupportSystemItems);
                NotSupportSystemGroup.AddIfNotNull(viewItem.NotSupportSystemItems);
            }

            // Build the final navigation items list
            if (!SupportSystemGroup.Empty() || !NotSupportSystemGroup.Empty()) {
                // Create the final result list
                var result = new List<object>() {
                    new NavigationViewItemHeader() {
                        Content = App.GetService<ResourceLoader>().GetString("NavItemConverter_SupportSystemGroupName")
                    }
                };

                // Add supported system group items
                result.AddRange(SupportSystemGroup);

                if (!NotSupportSystemGroup.Empty()) {
                    // Add not supported system group items if any
                    result.Add(new NavigationViewItemHeader() {
                        Content = App.GetService<ResourceLoader>().GetString("NavItemConverter_NotSupportSystemGroupName")
                    });

                    // Add not supported system group items
                    result.AddRange(NotSupportSystemGroup);
                }

                MainNavigationView.MenuItems.AddRange(result);
            }
        }

        throw new InvalidOperationException("The provided collection contains no items.");
    }

    // Helper method to create navigation items for a group
    private (NavigationViewItem? SupportSystemItems, NavigationViewItem? NotSupportSystemItems) CreateNavItem(ScriptGroup groupInfo, IEnumerable<LabScriptItem> items) {
        // Initialize lists to hold supported and not supported system items
        var SupportSystemItems = new List<NavigationViewItem>();
        var NotSupportSystemItems = new List<NavigationViewItem>();

        // Iterate through each item in the group
        foreach (var item in items) {
            // Add items to respective lists based on system support
            if (item.SupportedSystems.Contains(App.GetCurrentSystem())) {
                SupportSystemItems.Add(new NavigationViewItem() {
                    Content = item.Title,
                    Icon = item.Icon,
                    InfoBadge = item.Info,
                    Tag = item.Id
                });
            }
            else {
                NotSupportSystemItems.Add(new NavigationViewItem() {
                    Content = item.Title,
                    Icon = item.Icon,
                    InfoBadge = item.Info,
                    Tag = item.Id
                });
            }
        }

        // Create group navigation items
        NavigationViewItem? SupportSystemGroup = null;
        NavigationViewItem? NotSupportSystemGroup = null;

        // Create group items if there are any items in the respective lists
        if (!SupportSystemItems.Empty()) {
            SupportSystemGroup = new NavigationViewItem() {
                Content = groupInfo.Name,
                Icon = groupInfo.Icon,
                SelectsOnInvoked = false
            };

            SupportSystemGroup.MenuItems.AddRange(SupportSystemItems);
        }

        if (!NotSupportSystemItems.Empty()) {
            NotSupportSystemGroup = new NavigationViewItem() {
                Content = groupInfo.Name,
                Icon = groupInfo.Icon,
                SelectsOnInvoked = false
            };

            NotSupportSystemGroup.MenuItems.AddRange(NotSupportSystemItems);
        }

        return (SupportSystemGroup, NotSupportSystemGroup);
    }
#endregion

}
