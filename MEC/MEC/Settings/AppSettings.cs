namespace MEC.Settings;

// TODO: Maybe use a different name. ASP uses this name. Perhaps "MECSettings" or just "Settings".
public sealed class AppSettings : ISetting
{
    public GeneralSettings GeneralSettings { get; init; } = new();
    public ProviderSettings ProviderSettings { get; init; } = new();

    public bool IsValid()
    {
        return (GeneralSettings?.IsValid() ?? false) && (ProviderSettings?.IsValid() ?? false);
    }
}