using System;
using System.Collections.Generic;
using System.Text;
using RwagLab.Models.Enums;

namespace RwagLab.Models.Classes;

public struct LabSpriptItem {
    public required string Title { get; init; }

    public required IconSource Icon { get; init; }

    public required Type NavigatePage { get; init; }

    public required SupportedSystemEnum SupportedSystems { get; init; }
}
