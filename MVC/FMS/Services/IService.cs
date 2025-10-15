using FMS.Repositories;
using System.Linq.Expressions;

namespace FMS.Services;

/// <summary>
/// Generic service interface for business logic and CRUD operations on entities.
/// </summary>
/// <typeparam name="T">The entity type, which must be a class and IRepositoryEntity.</typeparam>
public interface IService<T> where T : class, IRepositoryEntity
{
    /// <summary>
    /// Checks if any entity matching the specified filter exists asynchronously.
    /// </summary>
    /// <param name="filterPredicate">An expression to filter the entities.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, returning true if at least one entity matches the filter, false otherwise.</returns>
    Task<bool> ExistAsync(Expression<Func<T, bool>> filterPredicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all entities asynchronously, with optional filtering and ordering.
    /// </summary>
    /// <param name="filterPredicate">An optional expression to filter the entities.</param>
    /// <param name="orderByPredicate">An optional expression to order the entities.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a collection of entities.</returns>
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filterPredicate = default, Expression<Func<T, object>>? orderByPredicate = default, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single entity asynchronously based on an optional filter.
    /// </summary>
    /// <param name="filterPredicate">An optional expression to filter the entity.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the matching entity or null if not found.</returns>
    Task<T> GetSingleAsync(Expression<Func<T, bool>>? filterPredicate = default, CancellationToken ct = default);

    /// <summary>
    /// Creates a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateAsync(T entity, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(T entity, CancellationToken ct = default);

    /// <summary>
    /// Deletes entities matching the specified filter asynchronously.
    /// </summary>
    /// <param name="filterPredicate">An optional expression to filter the entities to delete.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(Expression<Func<T, bool>>? filterPredicate = default, CancellationToken ct = default);
}