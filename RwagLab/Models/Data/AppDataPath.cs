using System;
using System.Collections.Generic;
using System.Text;

namespace RwagLab.Models.Data;

public static class AppDataPath {
    public static readonly string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RwagLab");

    public static readonly string VersionConfigsPath = Path.Combine(dataPath, "LaunchConfigs");

    public static readonly Dictionary<string, string> pathsList = new Dictionary<string, string>() {
        { "ConfigsPath", Path.Combine(dataPath, "Configs") },
        { "LogsPath", Path.Combine(dataPath, "Logs") },
        { "AssetsPath", Path.Combine(dataPath, "Assets") }
    };
}
