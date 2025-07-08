using AirWeb.Domain.Comments;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.EfRepository.Contexts;

namespace AirWeb.EfRepository.Repositories;

public sealed class WorkEntryRepository(AppDbContext context)
    : BaseRepository<WorkEntry, int, AppDbContext>(context), IWorkEntryRepository
{
    // Entity Framework will set the ID automatically.
    public int? GetNextId() => null;

    public Task<TEntry?> FindAsync<TEntry>(int id, bool includeComments, CancellationToken token = default)
        where TEntry : WorkEntry
    {
        var query = Context.Set<TEntry>().AsNoTracking();
        var include = includeComments
            ? query.Include(entry => entry.Comments
                .Where(comment => !comment.DeletedAt.HasValue)
                .OrderBy(comment => comment.CommentedAt)
                .ThenBy(comment => comment.Id))
            : query;
        return include.SingleOrDefaultAsync(entry => entry.Id.Equals(id), token);
    }

    public Task<WorkEntryType> GetWorkEntryTypeAsync(int id, CancellationToken token = default) =>
        Context.Set<WorkEntry>().AsNoTracking()
            .Where(entry => entry.Id.Equals(id)).Select(entry => entry.WorkEntryType).SingleAsync(token);

    public Task<bool> SourceTestReviewExistsAsync(int referenceNumber, CancellationToken token = default) =>
        Context.Set<SourceTestReview>().AsNoTracking()
            .AnyAsync(str => str.ReferenceNumber.Equals(referenceNumber) && !str.IsDeleted, token);

    public Task<SourceTestReview?> FindSourceTestReviewAsync(int referenceNumber, CancellationToken token = default) =>
        Context.Set<SourceTestReview>().AsNoTracking()
            .SingleOrDefaultAsync(str => str.ReferenceNumber.Equals(referenceNumber) && !str.IsDeleted, token);

    public Task<NotificationType> GetNotificationTypeAsync(Guid typeId, CancellationToken token = default) =>
        Context.Set<NotificationType>().AsNoTracking()
            .SingleAsync(notificationType => notificationType.Id.Equals(typeId), cancellationToken: token);

    public async Task AddCommentAsync(int itemId, Comment comment, CancellationToken token = default)
    {
        Context.WorkEntryComments.Add(new WorkEntryComment(comment, itemId));
        await SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task DeleteCommentAsync(Guid commentId, string? userId, CancellationToken token = default)
    {
        var comment = await Context.WorkEntryComments.FindAsync([commentId], token).ConfigureAwait(false);
        if (comment != null)
        {
            comment.SetDeleted(userId);
            await SaveChangesAsync(token).ConfigureAwait(false);
        }
    }
}
