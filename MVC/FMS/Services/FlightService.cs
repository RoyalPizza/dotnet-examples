using FMS.Db;
using FMS.Repositories;
using System.Linq.Expressions;

namespace FMS.Services;

/// <summary>
/// Service for managing Flight entities.
/// </summary>
public class FlightService : IService<Flight>
{
    private readonly IRepository<Flight> _repository;
    private readonly ILogger<CarrierService> _logger;

    /// <summary>
    /// Initializes a new instance of the CarrierService class.
    /// </summary>
    /// <param name="repository">The repository for Flight entities.</param>
    /// <param name="logger">The logger instance.</param>
    public FlightService(IRepository<Flight> repository, ILogger<CarrierService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Checks if any Flight matching the specified filter exists asynchronously.
    /// </summary>
    /// <param name="filterPredicate">An expression to filter the carriers.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, returning true if at least one flight matches the filter, false otherwise.</returns>
    public async Task<bool> ExistAsync(Expression<Func<Flight, bool>> filterPredicate, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Service: Checking if flight exists with filter");
        return await _repository.ExistAsync(filterPredicate, cancellationToken);
    }

    /// <summary>
    /// Retrieves all Flight entities asynchronously, with optional filtering and ordering.
    /// </summary>
    /// <param name="filterPredicate">An optional expression to filter the carriers.</param>
    /// <param name="orderByPredicate">An optional expression to order the carriers.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a collection of carriers.</returns>
    public async Task<IEnumerable<Flight>> GetAllAsync(Expression<Func<Flight, bool>>? filterPredicate = default, Expression<Func<Flight, object>>? orderByPredicate = default, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Retrieving all carriers");
        return await _repository.GetAllAsync(filterPredicate, orderByPredicate, ct);
    }

    /// <summary>
    /// Retrieves a single Flight asynchronously based on an optional filter.
    /// </summary>
    /// <param name="filterPredicate">An optional expression to filter the flight.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the matching flight or null if not found.</returns>
    public async Task<Flight> GetSingleAsync(Expression<Func<Flight, bool>>? filterPredicate = default, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Retrieving single flight with filter");
        var flight = await _repository.GetSingleAsync(filterPredicate, ct);
        if (flight == null)
        {
            _logger.LogWarning("Service: No flight found matching the filter");
        }
        return flight;
    }

    /// <summary>
    /// Creates a new Flight asynchronously.
    /// </summary>
    /// <param name="flight">The flight to create.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when a flight with the same name already exists.</exception>
    public async Task CreateAsync(Flight flight, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Adding new flight: {Name}", flight.GetName());
        if (await ExistAsync(c => c.GetName() == flight.GetName(), ct))
        {
            _logger.LogWarning("Service: Flight with name {Name} already exists", flight.GetName());
            throw new ArgumentException("A flight with this name already exists.", "Name");
        }
        await _repository.CreateAsync(flight, ct);
        _logger.LogInformation("Service: Flight {Name} added successfully", flight.GetName());
    }

    /// <summary>
    /// Updates an existing Flight asynchronously.
    /// </summary>
    /// <param name="flight">The flight to update.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateAsync(Flight flight, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Updating flight: {Name}", flight.GetName());
        await _repository.UpdateAsync(flight, ct);
        _logger.LogInformation("Service: Flight {Name} updated successfully", flight.GetName());
    }

    /// <summary>
    /// Deletes carriers matching the specified filter asynchronously.
    /// </summary>
    /// <param name="filterPredicate">An optional expression to filter the carriers to delete.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteAsync(Expression<Func<Flight, bool>>? filterPredicate = default, CancellationToken ct = default)
    {
        _logger.LogInformation("Service: Deleting carriers with filter");
        await _repository.DeleteAsync(filterPredicate, ct);
        _logger.LogInformation("Service: Flights deleted successfully");
    }
}