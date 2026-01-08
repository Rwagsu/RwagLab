using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using RwagLab.Models.Enums;

namespace RwagLab.Models.Classes;

public class LabScriptItem {
    public required string Id { get; init; }

    public required string Title { get; init; }

    public required IconElement Icon { get; init; }

    public InfoBadge? Info { get; init; }

    public required Type NavigatePage { get; init; }

    public required ReadOnlyCollection<SupportedSystemEnum> SupportedSystems { get; init; }

    public required string NotSupportedSystemTip { get; init; }
}
