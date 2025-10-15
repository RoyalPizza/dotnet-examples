using FMS.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FMS.Repositories.EF
{
    public abstract class EFCRUDRepository<T> : IRepository<T> where T : class, IRepositoryEntity
    {
        protected readonly AppDbContext _context;
        protected readonly ILogger<EFCarrierRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the CarrierRepository class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public EFCRUDRepository(AppDbContext context, ILogger<EFCarrierRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Checks if any Entity matching the specified filter exists asynchronously.
        /// </summary>
        /// <param name="filterPredicate">An expression to filter the entities.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning true if at least one entity matches the filter, false otherwise.</returns>
        public async Task<bool> ExistAsync(Expression<Func<T, bool>> filterPredicate, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Checking if entity exists with filter");
            return await _context.Set<T>().AnyAsync(filterPredicate, cancellationToken);
        }

        /// <summary>
        /// Retrieves all Entity entities asynchronously, with optional filtering and ordering.
        /// </summary>
        /// <param name="filterPredicate">An optional expression to filter the entities.</param>
        /// <param name="orderByPredicate">An optional expression to order the entities.</param>
        /// <param name="ct">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, containing a collection of entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filterPredicate = default, Expression<Func<T, object>>? orderByPredicate = default, CancellationToken ct = default)
        {
            _logger.LogInformation("Retrieving all entities");
            var query = _context.Set<T>().AsQueryable();

            if (filterPredicate != null)
            {
                query = query.Where(filterPredicate);
            }

            if (orderByPredicate != null)
            {
                query = query.OrderBy(orderByPredicate);
            }

            return await query.ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves a single Entity asynchronously based on an optional filter.
        /// </summary>
        /// <param name="filterPredicate">An optional expression to filter the entity.</param>
        /// <param name="ct">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, containing the matching entity or null if not found.</returns>
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>>? filterPredicate = default, CancellationToken ct = default)
        {
            _logger.LogInformation("Retrieving single entity with filter");
            var query = _context.Set<T>().AsQueryable();

            if (filterPredicate != null)
            {
                query = query.Where(filterPredicate);
            }

            return await query.FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Creates a new Entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <param name="ct">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateAsync(T entity, CancellationToken ct = default)
        {
            _logger.LogInformation("Adding new entity: {Name}", entity.GetName());
            await _context.Set<T>().AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Entity {Name} added successfully", entity.GetName());
        }

        /// <summary>
        /// Updates an existing Entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="ct">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _logger.LogInformation("Updating entity: {Name}", entity.GetName());
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Entity {Name} updated successfully", entity.GetName());
        }

        /// <summary>
        /// Deletes entities matching the specified filter asynchronously.
        /// </summary>
        /// <param name="filterPredicate">An optional expression to filter the entities to delete.</param>
        /// <param name="ct">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(Expression<Func<T, bool>>? filterPredicate = default, CancellationToken ct = default)
        {
            _logger.LogInformation("Deleting entities with filter");
            var query = _context.Set<T>().AsQueryable();

            if (filterPredicate != null)
            {
                query = query.Where(filterPredicate);
            }

            var entities = await query.ToListAsync(ct);
            if (!entities.Any())
            {
                _logger.LogWarning("No entities found matching the filter for deletion");
                return;
            }

            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Deleted {Count} entities successfully", entities.Count);
        }
    }
}
