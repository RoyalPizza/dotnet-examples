using FMS.Repositories;

namespace FMS.Db;

public class Carrier : IRepositoryEntity
{
    /// <summary>
    ///     Auto-incremented ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The name of the airline. (ex. Delta)
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    ///     The IATA code. (ex. 006)
    /// </summary>
    public string Code { get; set; } = "";

    public string GetName()
    {
        return Name;
    }
}