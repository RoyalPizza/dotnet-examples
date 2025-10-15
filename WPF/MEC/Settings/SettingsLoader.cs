using System.IO;
using System.Text.Json;

namespace MEC.Settings;

public static class SettingsLoader
{
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public static T LoadOrCreate<T>(string filePath) where T : new()
    {
#if DEBUG
        // TODO: WPF will call the app host constructor at design time.
        // This function is called in that constructor.
        // This is a possible workaround, but not 100% it will work as desired.
        // The view models need the services to exist, so we cannot prevent app host entirely.
        // We dont want appsettings.json created at the solution directory, so just return this blank object.
        // Just verify this covers the conditions I care about

        if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            return Activator.CreateInstance<T>();
#endif

        if (!File.Exists(filePath))
        {
            var defaultSettings = new T();
            Save(filePath, defaultSettings);
            return defaultSettings;
        }

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json, _options) ?? new T();
    }

    public static void Save<T>(string filePath, T settings)
    {
#if DEBUG
        // TODO: See comment in LoadOrCreate
        if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            return;
#endif

        var json = JsonSerializer.Serialize(settings, _options);
        File.WriteAllText(filePath, json);
    }
}