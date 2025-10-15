using FMS.Db;

namespace FMS.Repositories.EF;

/// <summary>
/// Repository for managing Pier entities.
/// </summary>
public class EFPierRepository : EFCRUDRepository<Pier>
{
    public EFPierRepository(AppDbContext context, ILogger<EFCarrierRepository> logger) : base(context, logger)
    {
    }
}