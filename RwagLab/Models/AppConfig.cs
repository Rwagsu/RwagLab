using Windows.Foundation;
using Windows.Graphics;
using System.Collections.Immutable;

namespace RwagLab.Models;

public record AppConfig {
    public string Environment { get; init; } = string.Empty;

    public string ApplicationName { get; init; } = string.Empty;

    public string Owner { get; init; } = string.Empty;

    public string OwnerLink { get; init; } = string.Empty;

    public int WindowHeight { get; init; }

    public int WindowWidth { get; init; }

    public string BingWallpaperUrl { get; init; } = string.Empty;

    public Dictionary<string, string> SupportedLanguages { get; init; } = new Dictionary<string, string>();
}
