namespace MEC.Db;

public class Carrier
{
    /// <summary>
    ///     Auto-incremented ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     The name of the airline. (ex. Delta)
    /// </summary>
    public string Name { get; init; } = "";

    /// <summary>
    ///     The IATA code. (ex. 006)
    /// </summary>
    public string Code { get; init; } = "";
}