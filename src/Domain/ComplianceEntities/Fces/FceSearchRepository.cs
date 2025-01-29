using AirWeb.Domain.ComplianceEntities.Search;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace AirWeb.Domain.ComplianceEntities.Fces;

public sealed class FceSearchRepository(IFceRepository repository)
    : ComplianceSearchRepository<Fce>(repository), IFceSearchRepository
{
    public Task<IReadOnlyCollection<Fce>> GetFilteredRecordsAsync(Expression<Func<Fce, bool>> expression,
        PaginatedRequest paging, CancellationToken token = default) =>
        GetRecordsAsync(expression, paging, token);

    public Task<IReadOnlyCollection<Fce>> GetFilteredRecordsAsync(Expression<Func<Fce, bool>> expression,
        string sorting, CancellationToken token = default) =>
        GetRecordsAsync(expression, sorting, token);

    public Task<int> CountRecordsAsync(Expression<Func<Fce, bool>> expression, CancellationToken token = default) =>
        CountAsync(expression, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public ValueTask DisposeAsync() => repository.DisposeAsync();

    #endregion
}
