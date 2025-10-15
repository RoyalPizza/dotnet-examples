using FMS.Db;

namespace FMS.Models;

public class CarrierViewModel
{
    public IEnumerable<Carrier> Carriers { get; set; } = Enumerable.Empty<Carrier>();
}