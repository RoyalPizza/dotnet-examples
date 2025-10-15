using FMS.Db;

namespace FMS.Repositories.EF;

/// <summary>
/// Repository for managing Carrier entities.
/// </summary>
public class EFCarrierRepository : EFCRUDRepository<Carrier>
{
    public EFCarrierRepository(AppDbContext context, ILogger<EFCarrierRepository> logger) : base(context, logger)
    {
    }
}