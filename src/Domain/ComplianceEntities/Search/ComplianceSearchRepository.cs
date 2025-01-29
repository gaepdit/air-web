using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace AirWeb.Domain.ComplianceEntities.Search;

public abstract class ComplianceSearchRepository<TEntity>(IRepository<TEntity, int> repository)
    where TEntity : IEntity<int>
{
    protected Task<IReadOnlyCollection<TEntity>> GetRecordsAsync(Expression<Func<TEntity, bool>> expression,
        PaginatedRequest paging, CancellationToken token = default) =>
        repository.GetPagedListAsync(expression, paging, token);

    protected async Task<IReadOnlyCollection<TEntity>> GetRecordsAsync(
        Expression<Func<TEntity, bool>> expression, string sorting, CancellationToken token = default) =>
        (await repository.GetListAsync(expression, token).ConfigureAwait(false)).AsQueryable().OrderByIf(sorting)
        .ToList();

    protected Task<int> CountAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken token = default) =>
        repository.CountAsync(expression, token);
}
