namespace MEC.Core;

public class LogFilename
{
    public string Filename { get; init; } = "";
    public DateOnly Date { get; set; }
    public override string ToString()
    {
        return Date.ToString();
    }
}
