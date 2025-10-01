using AirWeb.Domain.Comments;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.NamedEntities;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalWorkEntryRepository()
    : BaseRepositoryWithMapping<WorkEntry, int>(WorkEntryData.GetData), IWorkEntryRepository
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
}
