using FMS.Db;

namespace FMS.Models;

public class PierViewModel
{
    public IEnumerable<Pier> Piers { get; set; } = Enumerable.Empty<Pier>();
}
