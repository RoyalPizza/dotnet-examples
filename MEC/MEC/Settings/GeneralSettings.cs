namespace MEC.Settings;

public sealed class GeneralSettings : ISetting
{
    public string StationName { get; init; } = "MEC-01";

    public bool IsValid()
    {
        var stringsValid = !string.IsNullOrEmpty(StationName);

        return stringsValid;
    }
}