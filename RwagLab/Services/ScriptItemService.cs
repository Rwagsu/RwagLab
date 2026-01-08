using System.Collections.ObjectModel;
using RwagLab.Models.Classes;
using RwagLab.Models.Enums;
using RwagLab.Views.Pages.Scripts.AppInstaller;

namespace RwagLab.Services;

public class ScriptItemService {
    private readonly ResourceLoader resourceLoader;

    public ScriptItemService() {
        // Initialize ResourceLoader
        resourceLoader = ResourceLoader.GetForViewIndependentUse("ScriptNames");

        // Initialize LabScriptItems
        var content = new Dictionary<ScriptGroup, List<LabScriptItem>> {
            // SystemTools Group
            {
                new ScriptGroup {
                    GroupID = "SystemTools",
                    Name = resourceLoader.GetString("SystemTools_GroupName") ?? string.Empty,
                    Icon = new FontIcon { Glyph = "\uE770" },
                },
                [
                    // AppInstaller Item
                    new LabScriptItem {
                        Id = "AppInstaller",
                        Title = resourceLoader.GetString("AppInstaller_Title") ?? string.Empty,
                        Icon = new FontIcon { Glyph = "\uE896" },
                        NavigatePage = typeof(AppInstallerPage),
                        SupportedSystems = [SupportedSystemEnum.Windows],
                        NotSupportedSystemTip = resourceLoader.GetString("AppInstaller_SystemTip") ?? string.Empty,
                    }
                ]
            },
        };

        LabScriptItems = new ReadOnlyDictionary<ScriptGroup, List<LabScriptItem>>(content);
          
    }

    public ReadOnlyDictionary<ScriptGroup, List<LabScriptItem>> LabScriptItems { get; init; }
}
