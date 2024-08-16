using AirWeb.Domain.ComplianceEntities;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.Search;
using AirWeb.TestData.Entities;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalComplianceSearchRepository : IComplianceSearchRepository
{
    internal ICollection<WorkEntry> WorkEntryItems { get; } = WorkEntryData.GetData.ToList();
    internal ICollection<Fce> FceItems { get; } = FceData.GetData.ToList();

    public Task<IReadOnlyCollection<TEntity>> GetFilteredRecordsAsync<TEntity>(
        Expression<Func<TEntity, bool>> expression, PaginatedRequest paging, CancellationToken token = default)
        where TEntity : class, IEntity<int>, IComplianceEntity
    {
        var items = (typeof(TEntity) switch
        {
            var type when type == typeof(WorkEntry) => (ICollection<TEntity>)WorkEntryItems,
            var type when type == typeof(Fce) => (ICollection<TEntity>)FceItems,
            _ => throw new ArgumentOutOfRangeException(nameof(expression)),
        }).Where(expression.Compile()).ToArray();

        foreach (var entity in items)
            entity.Facility = FacilityData.GetData.Single(facility => facility.Id == entity.FacilityId);

        return Task.FromResult<IReadOnlyCollection<TEntity>>(items.AsQueryable()
            .OrderByIf(paging.Sorting)
            .Skip(paging.Skip).Take(paging.Take)
            .ToList());
    }

    public Task<int> CountRecordsAsync<TEntity>(
        Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
        where TEntity : class, IEntity<int>
    {
        var items = typeof(TEntity) switch
        {
            var type when type == typeof(WorkEntry) => (ICollection<TEntity>)WorkEntryItems,
            var type when type == typeof(Fce) => (ICollection<TEntity>)FceItems,
            _ => throw new ArgumentOutOfRangeException(nameof(expression)),
        };

        return Task.FromResult(items.Count(expression.Compile()));
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        // Method intentionally left empty.
    }

    public ValueTask DisposeAsync() => default;

    #endregion
}
