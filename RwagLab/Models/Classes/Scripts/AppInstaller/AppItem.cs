using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace RwagLab.Models.Classes.Scripts.AppInstaller;

public class AppItem {
    public required string Name { get; set; }

    public required string ID { get; set; }

    public required string AppPath { get; set; }

    public required string AppShortcutPath { get; set; }
}
