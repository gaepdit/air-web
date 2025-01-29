using AirWeb.Domain.ComplianceEntities.Search;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public sealed class WorkEntrySearchRepository(IWorkEntryRepository repository)
    : ComplianceSearchRepository<WorkEntry>(repository), IWorkEntrySearchRepository
{
    public Task<IReadOnlyCollection<WorkEntry>> GetFilteredRecordsAsync(Expression<Func<WorkEntry, bool>> expression,
        PaginatedRequest paging, CancellationToken token = default) =>
        GetRecordsAsync(expression, paging, token);

    public Task<IReadOnlyCollection<WorkEntry>> GetFilteredRecordsAsync(Expression<Func<WorkEntry, bool>> expression,
        string sorting, CancellationToken token = default) =>
        GetRecordsAsync(expression, sorting, token);

    public Task<int> CountRecordsAsync(Expression<Func<WorkEntry, bool>> expression,
        CancellationToken token = default) =>
        CountAsync(expression, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public ValueTask DisposeAsync() => repository.DisposeAsync();

    #endregion
}
