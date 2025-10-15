using FMS_API.Db;

namespace FMS_API.Repositories.EF;

/// <summary>
/// Repository for managing Carrier entities.
/// </summary>
public class EFCarrierRepository : EFCRUDRepository<Carrier>
{
    public EFCarrierRepository(AppDbContext context, ILogger<EFCarrierRepository> logger) : base(context, logger)
    {
    }
}