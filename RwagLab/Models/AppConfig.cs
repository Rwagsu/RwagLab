using Windows.Foundation;
using Windows.Graphics;

namespace RwagLab.Models;

public record AppConfig {
    public string? Environment { get; init; }

    public string? ApplicationName { get; init; }

    public int WindowHeight { get; init; }

    public int WindowWidth { get; init; }
}
