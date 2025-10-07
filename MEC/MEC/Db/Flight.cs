namespace MEC.Db;

public class Flight
{
    /// <summary>
    ///     Auto-incremented ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     The flight number for the flight.
    ///     Not unique. Max 12 characters.
    /// </summary>
    public string FlightNumber { get; init; } = "";

    /// <summary>
    ///     The key into the carrier table.
    /// </summary>
    public int CarrierKey { get; init; }

    /// <summary>
    ///     the code for the airport the flight is going to (ex. ATL)
    /// </summary>
    public string AirportCode { get; init; } = "";

    /// <summary>
    ///     The key into the pier table.
    /// </summary>
    public int PierKey { get; init; }

    /// <summary>
    ///     The time the flight departs.
    /// </summary>
    public DateTime DepartTime { get; init; } = DateTime.MinValue;
}