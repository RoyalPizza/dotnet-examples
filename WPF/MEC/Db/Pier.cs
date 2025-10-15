namespace MEC.Db;

public class Pier
{
    /// <summary>
    ///     Auto-incremented ID.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     The name of the pier.
    /// </summary>
    public string Name { get; init; } = "";

    /// <summary>
    ///     The sort code for the pier. Unique to the pier.
    /// </summary>
    public int Code { get; init; } = 0;
}