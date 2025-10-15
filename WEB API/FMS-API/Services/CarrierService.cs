using FMS_API.Db;
using FMS_API.Repositories;
using System.Linq.Expressions;

namespace FMS_API.Services;

/// <summary>
/// Service for managing Carrier entities.
/// </summary>
public class CarrierService : IService<Carrier>
{
    private readonly IRepository<Carrier> _repository;
    private readonly ILogger<CarrierService> _logger;

    /// <summary>
    /// Initializes a new instance of the CarrierService class.
    /// </summary>
    /// <param name="repository">The repository for Carrier entities.</param>
    /// <param name="logger">The logger instance.</param>
    public CarrierService(IRepository<Carrier> repository, ILogger<CarrierService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Checks if any Carrier matching the specified filter exists asynchronously.
    /// </summary>
    /// <param name="filterPredicate">An expression to filter the carriers.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, returning true if at least one carrier matches the filter, false otherwise.</returns>
    public async Task<bool> ExistAsync(Expression<Func<Carrier, bool>> filterPredicate, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Service: Checking if carrier exists with filter");
        return await _repository.ExistAsync(filterPredicate, cancellationToken);
    }

    /// <summary>
    /// Retrieves all Carrier entities asynchronously, with optional filtering and ordering.
    /// </summary>
    /// <param name="filterPredicate">An optional expression to filter the carriers.</param>
    /// <param name="orderByPredicate">An optional expression to order the carriers.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a collection of carriers.</returns>
    public async Task<IEnumerable<Carrier>> GetAllAsync(Expression<Func<Carrier, bool>>? filterPredicate = default, Expression<Func<Carrier, object>>? orderByPredicate = default, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Retrieving all carriers");
        return await _repository.GetAllAsync(filterPredicate, orderByPredicate, ct);
    }

    /// <summary>
    /// Retrieves a single Carrier asynchronously based on an optional filter.
    /// </summary>
    /// <param name="filterPredicate">An optional expression to filter the carrier.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the matching carrier or null if not found.</returns>
    public async Task<Carrier> GetSingleAsync(Expression<Func<Carrier, bool>>? filterPredicate = default, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Retrieving single carrier with filter");
        var carrier = await _repository.GetSingleAsync(filterPredicate, ct);
        if (carrier == null)
        {
            _logger.LogWarning("Service: No carrier found matching the filter");
        }
        return carrier;
    }

    /// <summary>
    /// Creates a new Carrier asynchronously.
    /// </summary>
    /// <param name="carrier">The carrier to create.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when a carrier with the same name already exists.</exception>
    public async Task CreateAsync(Carrier carrier, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Adding new carrier: {Name}", carrier.Name);
        if (await ExistAsync(c => c.Name == carrier.Name, ct))
        {
            _logger.LogWarning("Service: Carrier with name {Name} already exists", carrier.Name);
            throw new ArgumentException("A carrier with this name already exists.", nameof(carrier.Name));
        }
        await _repository.CreateAsync(carrier, ct);
        _logger.LogInformation("Service: Carrier {Name} added successfully", carrier.Name);
    }

    /// <summary>
    /// Updates an existing Carrier asynchronously.
    /// </summary>
    /// <param name="carrier">The carrier to update.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateAsync(Carrier carrier, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Updating carrier: {Name}", carrier.Name);
        await _repository.UpdateAsync(carrier, ct);
        _logger.LogInformation("Service: Carrier {Name} updated successfully", carrier.Name);
    }

    /// <summary>
    /// Deletes carriers matching the specified filter asynchronously.
    /// </summary>
    /// <param name="filterPredicate">An optional expression to filter the carriers to delete.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteAsync(Expression<Func<Carrier, bool>>? filterPredicate = default, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Deleting carriers with filter");
        await _repository.DeleteAsync(filterPredicate, ct);
        _logger.LogInformation("Service: Carriers deleted successfully");
    }
}