using FMS.Db;

namespace FMS.Models;

public class FlightViewModel
{
    public IEnumerable<Flight> Flights { get; set; } = Enumerable.Empty<Flight>();
}