using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.ValueObjects;
using AirWeb.TestData;
using System.Linq.Expressions;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalWorkEntryRepository()
    : BaseRepository<BaseWorkEntry, int>(WorkEntryData.GetData), IWorkEntryRepository
{
    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Select(entry => entry.Id).Max() + 1;

    public Task<WorkEntryType> GetWorkEntryTypeAsync(int id, CancellationToken token = default) =>
        Task.FromResult(Items.Single(entry => entry.Id.Equals(id)).WorkEntryType);

    public Task<ComplianceEventType> GetComplianceEventTypeAsync(int id, CancellationToken token = default) =>
        Task.FromResult(((BaseComplianceEvent)Items.Single(entry => entry.Id.Equals(id))).ComplianceEventType);

    public async Task<TEntry?> FindAsync<TEntry>(int id, CancellationToken token = default)
        where TEntry : BaseWorkEntry =>
        (TEntry?)await FindAsync(id, token).ConfigureAwait(false);

    public Task<BaseWorkEntry?> FindAsync(Expression<Func<BaseWorkEntry, bool>> predicate, string[] includeProperties,
        CancellationToken token = default) =>
        FindAsync(predicate, token);

    public Task<BaseWorkEntry> GetAsync(int id, string[] includeProperties, CancellationToken token = default) =>
        GetAsync(id, token);

    public async Task AddCommentAsync(int id, Comment comment, CancellationToken token = default) =>
        (await GetAsync(id, token).ConfigureAwait(false)).Comments.Add(comment);
}
