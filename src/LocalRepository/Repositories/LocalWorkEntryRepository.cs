using AirWeb.Domain.Comments;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.NamedEntities;
using AutoMapper;
using GaEpd.AppLibrary.Extensions;
using System.Linq.Expressions;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalWorkEntryRepository()
    : BaseRepository<WorkEntry, int>(WorkEntryData.GetData), IWorkEntryRepository
{
    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Count == 0 ? 1 : Items.Select(entry => entry.Id).Max() + 1;

    public async Task<TEntry?> FindAsync<TEntry>(int id, bool includeExtras,
        CancellationToken token = default)
        where TEntry : WorkEntry =>
        (TEntry?)await FindAsync(id, token: token).ConfigureAwait(false);

    public Task<WorkEntryType> GetWorkEntryTypeAsync(int id, CancellationToken token = default) =>
        Task.FromResult(Items.Single(entry => entry.Id.Equals(id)).WorkEntryType);

    public Task<bool> SourceTestReviewExistsAsync(int referenceNumber, CancellationToken token = default) =>
        Task.FromResult(Items.OfType<SourceTestReview>()
            .Any(str => str.ReferenceNumber == referenceNumber && !str.IsDeleted));

    public Task<SourceTestReview?> FindSourceTestReviewAsync(int referenceNumber, CancellationToken token = default) =>
        Task.FromResult(Items.OfType<SourceTestReview>()
            .FirstOrDefault(str => str.ReferenceNumber == referenceNumber && !str.IsDeleted));

    public Task<NotificationType> GetNotificationTypeAsync(Guid typeId, CancellationToken token = default) =>
        Task.FromResult(NotificationTypeData.GetData.Single(notificationType => notificationType.Id.Equals(typeId)));

    public async Task<IReadOnlyCollection<TDestination>> GetListAsync<TDestination, TSource>(
        Expression<Func<TSource, bool>>? predicate, IMapper mapper, string? ordering = null,
        CancellationToken token = default) where TSource : WorkEntry =>
        await Task.FromResult(mapper.Map<IReadOnlyCollection<TDestination>>(Items.OfType<TSource>()
            .WhereIf(predicate)
            .OrderByIf(ordering)
            .ToList())).ConfigureAwait(false);

    public async Task AddCommentAsync(int itemId, Comment comment, CancellationToken token = default) =>
        (await GetAsync(itemId, token: token).ConfigureAwait(false)).Comments.Add(
            new WorkEntryComment(comment, itemId));

    public Task DeleteCommentAsync(Guid commentId, string? userId, CancellationToken token = default)
    {
        var comment = Items.SelectMany(entry => entry.Comments).FirstOrDefault(comment => comment.Id == commentId);

        if (comment != null)
        {
            var fce = Items.First(entry => entry.Comments.Contains(comment));
            fce.Comments.Remove(comment);
        }

        return Task.CompletedTask;
    }

    public new Task UpdateAsync(WorkEntry entity, bool autoSave = true, CancellationToken token = default)
    {
        SetEventDate(entity);
        return base.UpdateAsync(entity, autoSave, token);
    }

    public new Task InsertAsync(WorkEntry entity, bool autoSave = true, CancellationToken token = default)
    {
        SetEventDate(entity);
        return base.InsertAsync(entity, autoSave, token);
    }

    private static void SetEventDate(WorkEntry entry) =>
        entry.EventDate = entry.WorkEntryType switch
        {
            WorkEntryType.AnnualComplianceCertification => (entry as AnnualComplianceCertification)!.ReceivedDate,
            WorkEntryType.Inspection => DateOnly.FromDateTime((entry as Inspection)!.InspectionStarted),
            WorkEntryType.Notification => (entry as Notification)!.ReceivedDate,
            WorkEntryType.PermitRevocation => (entry as PermitRevocation)!.ReceivedDate,
            WorkEntryType.Report => (entry as Report)!.ReceivedDate,
            WorkEntryType.RmpInspection => DateOnly.FromDateTime((entry as RmpInspection)!.InspectionStarted),
            WorkEntryType.SourceTestReview => (entry as SourceTestReview)!.ReceivedByComplianceDate,
            _ => DateOnly.FromDateTime(entry.CreatedAt?.Date ?? DateTime.MinValue),
        };
}
