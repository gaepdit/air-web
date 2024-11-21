using AirWeb.Domain.ComplianceEntities;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace AirWeb.Domain.Search;

public interface IComplianceSearchRepository : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Returns a filtered, read-only collection of <see cref="TEntity"/> records matching the conditions
    /// of <param name="expression"></param>. Returns an empty collection if there are no matches.
    /// </summary>
    /// <param name="expression">The search conditions.</param>
    /// <param name="paging">A <see cref="PaginatedRequest"/> to define the paging options.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A collection of filtered search results.</returns>
    Task<IReadOnlyCollection<TEntity>> GetFilteredRecordsAsync<TEntity>(Expression<Func<TEntity, bool>> expression,
        PaginatedRequest paging, CancellationToken token = default)
        where TEntity : class, IEntity<int>, IComplianceEntity;

    /// <summary>
    /// Returns a filtered, read-only collection of <see cref="TEntity"/> records matching the conditions
    /// of <param name="expression"></param>. Returns an empty collection if there are no matches.
    /// </summary>
    /// <param name="expression">The search conditions.</param>
    /// <param name="sorting"></param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A collection of filtered search results.</returns>
    Task<IReadOnlyCollection<TEntity>> GetFilteredRecordsAsync<TEntity>(Expression<Func<TEntity, bool>> expression,
        string sorting, CancellationToken token = default)
        where TEntity : class, IEntity<int>, IComplianceEntity;

    /// <summary>
    /// Returns the count of <see cref="TEntity"/> records matching the conditions of
    /// <param name="expression"></param>. Returns zero if there are no matches.
    /// </summary>
    /// <param name="expression">The Work Entry search conditions.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The number of matching records.</returns>
    public Task<int> CountRecordsAsync<TEntity>(Expression<Func<TEntity, bool>> expression,
        CancellationToken token = default) where TEntity : class, IEntity<int>;
}
