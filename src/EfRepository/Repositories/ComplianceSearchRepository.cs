using AirWeb.Domain.ComplianceEntities;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Search;
using AirWeb.EfRepository.DbContext;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace AirWeb.EfRepository.Repositories;

public sealed class ComplianceSearchRepository(AppDbContext context, IFacilityRepository facilityRepository)
    : IComplianceSearchRepository
{
    public async Task<IReadOnlyCollection<TEntity>> GetFilteredRecordsAsync<TEntity>(
        Expression<Func<TEntity, bool>> expression, PaginatedRequest paging, CancellationToken token = default)
        where TEntity : class, IEntity<int>, IComplianceEntity
    {
        var items = await context.Set<TEntity>().AsNoTracking()
            .Where(expression)
            .OrderByIf(paging.Sorting)
            .Skip(paging.Skip)
            .Take(paging.Take)
            .ToListAsync(token).ConfigureAwait(false);

        foreach (var entity in items)
            entity.Facility = await facilityRepository.GetFacilityAsync((FacilityId)entity.FacilityId, token)
                .ConfigureAwait(false);

        return items;
    }

    public Task<int> CountRecordsAsync<TEntity>(
        Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
        where TEntity : class, IEntity<int> =>
        context.Set<TEntity>().CountAsync(expression, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => context.Dispose();
    public ValueTask DisposeAsync() => context.DisposeAsync();

    #endregion
}
