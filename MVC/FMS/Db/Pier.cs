using FMS.Repositories;

namespace FMS.Db;

public class Pier : IRepositoryEntity
{
    /// <summary>
    ///     Auto-incremented ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The name of the pier.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    ///     The sort code for the pier. Unique to the pier.
    /// </summary>
    public int Code { get; set; } = 0;

    public string GetName()
    {
        return Name;
    }
}