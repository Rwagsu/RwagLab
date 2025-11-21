using System;
using System.Collections.Generic;
using System.Text;
using RwagLab.Models.Enums;

namespace RwagLab.Models.Data.ServicesData;

internal static class SettingsServiceData {
    internal static AppTheme appColorTheme = AppTheme.System;

    internal static BackgroundTypeEnum backgroundType = BackgroundTypeEnum.BingWallpaper;

    internal static string backgroundImagePath = string.Empty;

    internal static Stretch backgroundImageStretch = Stretch.UniformToFill;
}
