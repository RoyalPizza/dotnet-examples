using FMS_API.Db;
using FMS_API.Repositories;
using System.Linq.Expressions;

namespace FMS_API.Services;

/// <summary>
/// Service for managing Pier entities.
/// </summary>
public class PierService : IService<Pier>
{
    private readonly IRepository<Pier> _repository;
    private readonly ILogger<CarrierService> _logger;

    /// <summary>
    /// Initializes a new instance of the CarrierService class.
    /// </summary>
    /// <param name="repository">The repository for Pier entities.</param>
    /// <param name="logger">The logger instance.</param>
    public PierService(IRepository<Pier> repository, ILogger<CarrierService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Checks if any Pier matching the specified filter exists asynchronously.
    /// </summary>
    /// <param name="filterPredicate">An expression to filter the carriers.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, returning true if at least one pier matches the filter, false otherwise.</returns>
    public async Task<bool> ExistAsync(Expression<Func<Pier, bool>> filterPredicate, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Service: Checking if pier exists with filter");
        return await _repository.ExistAsync(filterPredicate, cancellationToken);
    }

    /// <summary>
    /// Retrieves all Pier entities asynchronously, with optional filtering and ordering.
    /// </summary>
    /// <param name="filterPredicate">An optional expression to filter the carriers.</param>
    /// <param name="orderByPredicate">An optional expression to order the carriers.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a collection of carriers.</returns>
    public async Task<IEnumerable<Pier>> GetAllAsync(Expression<Func<Pier, bool>>? filterPredicate = default, Expression<Func<Pier, object>>? orderByPredicate = default, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Retrieving all carriers");
        return await _repository.GetAllAsync(filterPredicate, orderByPredicate, ct);
    }

    /// <summary>
    /// Retrieves a single Pier asynchronously based on an optional filter.
    /// </summary>
    /// <param name="filterPredicate">An optional expression to filter the pier.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the matching pier or null if not found.</returns>
    public async Task<Pier> GetSingleAsync(Expression<Func<Pier, bool>>? filterPredicate = default, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Retrieving single pier with filter");
        var pier = await _repository.GetSingleAsync(filterPredicate, ct);
        if (pier == null)
        {
            _logger.LogWarning("Service: No pier found matching the filter");
        }
        return pier;
    }

    /// <summary>
    /// Creates a new Pier asynchronously.
    /// </summary>
    /// <param name="pier">The pier to create.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when a pier with the same name already exists.</exception>
    public async Task CreateAsync(Pier pier, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Adding new pier: {Name}", pier.Name);
        if (await ExistAsync(c => c.Name == pier.Name, ct))
        {
            _logger.LogWarning("Service: Pier with name {Name} already exists", pier.Name);
            throw new ArgumentException("A pier with this name already exists.", nameof(pier.Name));
        }
        await _repository.CreateAsync(pier, ct);
        _logger.LogInformation("Service: Pier {Name} added successfully", pier.Name);
    }

    /// <summary>
    /// Updates an existing Pier asynchronously.
    /// </summary>
    /// <param name="pier">The pier to update.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateAsync(Pier pier, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Updating pier: {Name}", pier.Name);
        await _repository.UpdateAsync(pier, ct);
        _logger.LogInformation("Service: Pier {Name} updated successfully", pier.Name);
    }

    /// <summary>
    /// Deletes carriers matching the specified filter asynchronously.
    /// </summary>
    /// <param name="filterPredicate">An optional expression to filter the carriers to delete.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteAsync(Expression<Func<Pier, bool>>? filterPredicate = default, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Deleting carriers with filter");
        await _repository.DeleteAsync(filterPredicate, ct);
        _logger.LogInformation("Service: Carriers deleted successfully");
    }
}