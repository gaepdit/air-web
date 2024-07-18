using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.ValueObjects;
using AirWeb.TestData.Entities;
using System.Linq.Expressions;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalWorkEntryRepository()
    : BaseRepository<WorkEntry, int>(WorkEntryData.GetData), IWorkEntryRepository
{
    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Count == 0 ? 1 : Items.Select(entry => entry.Id).Max() + 1;

    public Task<WorkEntry> GetAsync(int id, string[] includeProperties, CancellationToken token = default) =>
        GetAsync(id, token);

    public Task<WorkEntry?> FindAsync(Expression<Func<WorkEntry, bool>> predicate, string[] includeProperties,
        CancellationToken token = default) =>
        FindAsync(predicate, token);

    public async Task<TEntry?> FindAsync<TEntry>(int id, CancellationToken token = default)
        where TEntry : WorkEntry =>
        (TEntry?)await FindAsync(id, token).ConfigureAwait(false);

    public Task<WorkEntryType> GetWorkEntryTypeAsync(int id, CancellationToken token = default) =>
        Task.FromResult(Items.Single(entry => entry.Id.Equals(id)).WorkEntryType);

    public Task<ComplianceEventType> GetComplianceEventTypeAsync(int id, CancellationToken token = default) =>
        Task.FromResult(((ComplianceEvent)Items.Single(entry => entry.Id.Equals(id))).ComplianceEventType);

    public Task<NotificationType> GetNotificationTypeAsync(Guid typeId, CancellationToken token = default) =>
        Task.FromResult(NotificationTypeData.GetData.Single(notificationType => notificationType.Id.Equals(typeId)));

    public async Task AddCommentAsync(int id, Comment comment, CancellationToken token = default) =>
        (await GetAsync(id, token).ConfigureAwait(false)).Comments.Add(comment);
}
