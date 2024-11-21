using AirWeb.Domain.ComplianceEntities;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.Search;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalComplianceSearchRepository(
    IFceRepository fceRepository,
    IWorkEntryRepository workEntryRepository)
    : IComplianceSearchRepository
{
    public Task<IReadOnlyCollection<TEntity>> GetFilteredRecordsAsync<TEntity>(
        Expression<Func<TEntity, bool>> expression, PaginatedRequest paging, CancellationToken token = default)
        where TEntity : class, IEntity<int>, IComplianceEntity =>
        (typeof(TEntity) switch
        {
            var type when type == typeof(WorkEntry) => (IReadRepository<TEntity, int>)workEntryRepository,
            var type when type == typeof(Fce) => (IReadRepository<TEntity, int>)fceRepository,
            _ => throw new ArgumentOutOfRangeException(nameof(expression)),
        })
        .GetPagedListAsync(expression, paging, token);

    public Task<IReadOnlyCollection<TEntity>> GetFilteredRecordsAsync<TEntity>(
        Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
        where TEntity : class, IEntity<int>, IComplianceEntity =>
        (typeof(TEntity) switch
        {
            var type when type == typeof(WorkEntry) => (IReadRepository<TEntity, int>)workEntryRepository,
            var type when type == typeof(Fce) => (IReadRepository<TEntity, int>)fceRepository,
            _ => throw new ArgumentOutOfRangeException(nameof(expression)),
        })
        .GetListAsync(expression, token);

    public Task<int> CountRecordsAsync<TEntity>(
        Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
        where TEntity : class, IEntity<int> =>
        (typeof(TEntity) switch
        {
            var type when type == typeof(WorkEntry) => (IReadRepository<TEntity, int>)workEntryRepository,
            var type when type == typeof(Fce) => (IReadRepository<TEntity, int>)fceRepository,
            _ => throw new ArgumentOutOfRangeException(nameof(expression)),
        }).CountAsync(expression, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        // Method intentionally left empty.
    }

    public ValueTask DisposeAsync() => default;

    #endregion
}
