using FMS.Repositories;

namespace FMS.Db;

public class Flight : IRepositoryEntity
{
    /// <summary>
    ///     Auto-incremented ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The flight number for the flight.
    ///     Not unique. Max 12 characters.
    /// </summary>
    public string FlightNumber { get; set; } = "";

    /// <summary>
    ///     The key into the carrier table.
    /// </summary>
    public int CarrierKey { get; set; }

    /// <summary>
    ///     the code for the airport the flight is going to (ex. ATL)
    /// </summary>
    public string AirportCode { get; set; } = "";

    /// <summary>
    ///     The key into the pier table.
    /// </summary>
    public int PierKey { get; set; }

    /// <summary>
    ///     The time the flight departs.
    /// </summary>
    public DateTime DepartTime { get; set; } = DateTime.MinValue;

    public string GetName()
    {
        return FlightNumber;
    }
}