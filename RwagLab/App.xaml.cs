using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.Windows.ApplicationModel.Resources;
using RwagLab.Services;
using RwagLab.Views.Pages;
using Uno.Extensions.Toolkit;
using Uno.Resizetizer;
using Windows.Graphics;
using Windows.UI.Notifications;

namespace RwagLab;

public partial class App : Application {
    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>

    public App() {
        this.InitializeComponent();
    }

    private static IOptions<AppConfig> configuration = default!;

    public static IOptions<AppConfig> Configuration => configuration ?? throw new InvalidOperationException("MyField not initialized yet");


    public static new App Current => (App)Application.Current;

    public Window? MainWindow { get; private set; }

    public IThemeService? ThemeService { get; private set; }
    internal IHost? Host { get; private set; }

    protected override void OnLaunched(LaunchActivatedEventArgs args) {
        var builder = this.CreateBuilder(args)
            .Configure(host => host
#if DEBUG
                // Switch to Development environment when running in DEBUG
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging(configure: (context, logBuilder) =>
                {
                    // Configure log levels for different categories of logging
                    logBuilder
                        .SetMinimumLevel(
                            context.HostingEnvironment.IsDevelopment() ?
                                LogLevel.Information :
                                LogLevel.Warning)

                        // Default filters for core Uno Platform namespaces
                        .CoreLogLevel(LogLevel.Warning);

                    // Uno Platform namespace filter groups
                    // Uncomment individual methods to see more detailed logging
                    //// Generic Xaml events
                    //logBuilder.XamlLogLevel(LogLevel.Debug);
                    //// Layout specific messages
                    //logBuilder.XamlLayoutLogLevel(LogLevel.Debug);
                    //// Storage messages
                    //logBuilder.StorageLogLevel(LogLevel.Debug);
                    //// Binding related messages
                    //logBuilder.XamlBindingLogLevel(LogLevel.Debug);
                    //// Binder memory references tracking
                    //logBuilder.BinderMemoryReferenceLogLevel(LogLevel.Debug);
                    //// DevServer and HotReload related
                    //logBuilder.HotReloadCoreLogLevel(LogLevel.Information);
                    //// Debug JS interop
                    //logBuilder.WebAssemblyLogLevel(LogLevel.Debug);

                }, enableUnoLogging: true)
                .UseSerilog(consoleLoggingEnabled: true, fileLoggingEnabled: true)
                .UseConfiguration(configure: configBuilder =>
                    configBuilder
                        .EmbeddedSource<App>(includeEnvironmentSettings: false)
                        .Section<AppConfig>()
                )
                .ConfigureServices((context, services) =>
                {
                    // TODO: Register your services
                    //services.AddSingleton<IMyService, MyService>();
                    services.AddSingleton<HttpClient>();
                    services.AddSingleton<ResourceLoader>();

                    services.AddSingleton<PathService>();
                    services.AddSingleton<SettingsService>();
                    services.AddSingleton<ScriptItemService>();
                })
            );
        MainWindow = builder.Window;

#if DEBUG
        MainWindow.UseStudio();
#endif
        MainWindow.SetWindowIcon();

        Host = builder.Build();

        // Configuration
        configuration = GetService<IOptions<AppConfig>>();

        // Window Settings
        MainWindow.AppWindow.Resize(new SizeInt32 { Height = Configuration.Value.WindowHeight, Width = Configuration.Value.WindowWidth});
        MainWindow.AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;

        ThemeService = MainWindow.GetThemeService();

        Task.Run(async () => {
            if (ThemeService != null) {
                await ThemeService.SetThemeAsync(GetService<SettingsService>().AppColorTheme);
            }
        });

#if WINAPPSDK_PACKAGED
        // TODO: 三大金刚直接炸飞天(物理)
        MainWindow.AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
        MainWindow.AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        MainWindow.AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
#endif

        // Do not repeat app initialization when the Window already has content,
        // just ensure that the window is active
        if (MainWindow.Content is not Frame rootFrame)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new Frame();

            // Place the frame in the current Window
            MainWindow.Content = rootFrame;
        }

        if (rootFrame.Content == null)
        {
            // When the navigation stack isn't restored navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            rootFrame.Navigate(typeof(MainPage), args.Arguments);
        }
        // Ensure the current window is active
        MainWindow.Activate();
    }

    //Get Service
    public static T GetService<T>() {
        var services = Current.Host?.Services;
        if (services != null) {
            var service = services.GetService<T>();
            if (service != null) {
                return service;
            }
        }

        throw new InvalidOperationException("Service not found.");
    }
}
