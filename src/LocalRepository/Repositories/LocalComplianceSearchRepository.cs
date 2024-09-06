using AirWeb.Domain.ComplianceEntities;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.Search;
using AirWeb.TestData.ExternalEntities;
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
    public async Task<IReadOnlyCollection<TEntity>> GetFilteredRecordsAsync<TEntity>(
        Expression<Func<TEntity, bool>> expression, PaginatedRequest paging, CancellationToken token = default)
        where TEntity : class, IEntity<int>, IComplianceEntity
    {
        var items = await (typeof(TEntity) switch
        {
            var type when type == typeof(WorkEntry) => (IReadRepository<TEntity, int>)workEntryRepository,
            var type when type == typeof(Fce) => (IReadRepository<TEntity, int>)fceRepository,
            _ => throw new ArgumentOutOfRangeException(nameof(expression)),
        }).GetListAsync(expression, token).ConfigureAwait(false);

        foreach (var entity in items)
            entity.Facility = FacilityData.GetData.Single(facility => facility.Id == entity.FacilityId);

        return items.AsQueryable()
            .OrderByIf(paging.Sorting)
            .Skip(paging.Skip).Take(paging.Take)
            .ToList();
    }

    public async Task<int> CountRecordsAsync<TEntity>(
        Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
        where TEntity : class, IEntity<int> =>
        await (typeof(TEntity) switch
        {
            var type when type == typeof(WorkEntry) => (IReadRepository<TEntity, int>)workEntryRepository,
            var type when type == typeof(Fce) => (IReadRepository<TEntity, int>)fceRepository,
            _ => throw new ArgumentOutOfRangeException(nameof(expression)),
        }).CountAsync(expression, token).ConfigureAwait(false);

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        // Method intentionally left empty.
    }

    public ValueTask DisposeAsync() => default;

    #endregion
}
