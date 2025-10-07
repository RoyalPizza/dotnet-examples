namespace MEC.Core;

public class DispatchHistory
{
    public required string Pseudo { get; init; }
    public required string IATA { get; init; }
    public required string DispatchType { get; init; }
    public required string Destination { get; init; }

    public override string ToString()
    {
        return $"{Pseudo} \t {IATA} \t {DispatchType} sort to {Destination}";
    }
}