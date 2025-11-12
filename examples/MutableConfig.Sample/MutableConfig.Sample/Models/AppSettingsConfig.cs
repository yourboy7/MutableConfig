namespace MutableConfig.Sample.Models;

public class AppSettingsConfig {
    public string AppName { get; set; }
    public string Version { get; set; } = "1.0.0";
    public bool EnableDebug { get; set; } = true;
    public string LogDirectory { get; set; } = "logs";
}