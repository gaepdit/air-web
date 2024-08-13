using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.ValueObjects;
using AirWeb.TestData.Entities;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalWorkEntryRepository()
    : BaseRepository<WorkEntry, int>(WorkEntryData.GetData), IWorkEntryRepository
{
    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Count == 0 ? 1 : Items.Select(entry => entry.Id).Max() + 1;

    public Task<WorkEntry> GetAsync(int id, string[] includeProperties, CancellationToken token = default) =>
        GetAsync(id, token);

    public Task<WorkEntry?> FindAsync(int id, string[] includeProperties, CancellationToken token = default) =>
        FindAsync(id, token);

    public async Task<TEntry?> FindAsync<TEntry>(int id, CancellationToken token = default)
        where TEntry : WorkEntry =>
        (TEntry?)await FindAsync(id, token).ConfigureAwait(false);

    public Task<TEntry?> FindWithCommentsAsync<TEntry>(int id, CancellationToken token = default)
        where TEntry : WorkEntry =>
        FindAsync<TEntry>(id, token);

    public Task<WorkEntryType> GetWorkEntryTypeAsync(int id, CancellationToken token = default) =>
        Task.FromResult(Items.Single(entry => entry.Id.Equals(id)).WorkEntryType);

    public Task<NotificationType> GetNotificationTypeAsync(Guid typeId, CancellationToken token = default) =>
        Task.FromResult(NotificationTypeData.GetData.Single(notificationType => notificationType.Id.Equals(typeId)));

    public async Task AddCommentAsync(int id, Comment comment, CancellationToken token = default) =>
        (await GetAsync(id, token).ConfigureAwait(false)).Comments.Add(new WorkEntryComment(comment, id));

    public new Task InsertAsync(WorkEntry entity, bool autoSave = true, CancellationToken token = default)
    {
        entity.EventDate = EventDate(entity);
        Items.Add(entity);
        return Task.CompletedTask;
    }

    private static DateOnly EventDate(WorkEntry entry) => entry.WorkEntryType switch
    {
        WorkEntryType.AnnualComplianceCertification => (entry as AnnualComplianceCertification)!.ReceivedDate,
        WorkEntryType.Inspection => DateOnly.FromDateTime((entry as Inspection)!.InspectionStarted),
        WorkEntryType.Notification => (entry as Notification)!.ReceivedDate,
        WorkEntryType.PermitRevocation => (entry as PermitRevocation)!.ReceivedDate,
        WorkEntryType.Report => (entry as Report)!.ReceivedDate,
        WorkEntryType.RmpInspection => DateOnly.FromDateTime((entry as RmpInspection)!.InspectionStarted),
        WorkEntryType.SourceTestReview => (entry as SourceTestReview)!.ReceivedByCompliance,
        _ => DateOnly.FromDateTime(entry.CreatedAt?.Date ?? DateTime.MinValue),
    };
}
