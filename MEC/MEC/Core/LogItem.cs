namespace MEC.Core;

public class LogItem
{
    public DateTime Timestamp { get; init; }
    public string Level { get; init; } = "";
    public string Message { get; init; } = "";

    public string FriendlyTimestamp => Timestamp.ToString("0:HH:mm:s");
}
