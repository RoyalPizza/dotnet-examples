using FMS_API.Db;

namespace FMS_API.Repositories.EF;

/// <summary>
/// Repository for managing Flight entities.
/// </summary>
public class EFFlightRepository : EFCRUDRepository<Flight>
{
    public EFFlightRepository(AppDbContext context, ILogger<EFCarrierRepository> logger) : base(context, logger)
    {
    }
}