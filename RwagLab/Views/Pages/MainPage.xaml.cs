using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml.Media.Animation;
using RwagLab.Models.Enums;
using RwagLab.ViewModels.Pages;

namespace RwagLab.Views.Pages;

public sealed partial class MainPage : Page {
    public MainPage() {
        this.InitializeComponent();
        DataContext = new MainPageViewModel();

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
}
