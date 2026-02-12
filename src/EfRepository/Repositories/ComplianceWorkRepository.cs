using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.EfRepository.Contexts;

namespace AirWeb.EfRepository.Repositories;

public sealed class ComplianceWorkRepository(AppDbContext context)
    : BaseRepositoryWithMapping<ComplianceWork, int, AppDbContext>(context), IComplianceWorkRepository
{
    // Entity Framework will set the ID automatically.
    public int? GetNextId() => null;

    public Task<TWork?> FindAsync<TWork>(int id, bool includeExtras, CancellationToken token = default)
        where TWork : ComplianceWork
    {
        var query = Context.Set<TWork>().AsNoTracking();
        var include = includeExtras
            ? query
                .Include(work => work.Comments
                    .Where(comment => !comment.DeletedAt.HasValue)
                    .OrderBy(comment => comment.CommentedAt)
                    .ThenBy(comment => comment.Id))
                .Include(work => work.AuditPoints
                    .OrderBy(audit => audit.When).ThenBy(audit => audit.Id))
            : query;
        return include.SingleOrDefaultAsync(work => work.Id.Equals(id), token);
    }

    public Task<ComplianceWorkType> GetComplianceWorkTypeAsync(int id, CancellationToken token = default) =>
        Context.Set<ComplianceWork>().AsNoTracking()
            .Where(work => work.Id.Equals(id)).Select(work => work.ComplianceWorkType).SingleAsync(token);

    public Task<bool> SourceTestReviewExistsAsync(int referenceNumber, CancellationToken token = default) =>
        Context.Set<SourceTestReview>().AsNoTracking()
            .AnyAsync(str => str.ReferenceNumber.Equals(referenceNumber) && !str.IsDeleted, token);

    public Task<SourceTestReview?> FindSourceTestReviewAsync(int referenceNumber, CancellationToken token = default) =>
        Context.Set<SourceTestReview>().AsNoTracking()
            .Include(review => review.Comments
                .Where(comment => !comment.DeletedAt.HasValue)
                .OrderBy(comment => comment.CommentedAt)
                .ThenBy(comment => comment.Id))
            .Include(review => review.AuditPoints
                .OrderBy(audit => audit.When).ThenBy(audit => audit.Id))
            .SingleOrDefaultAsync(str => str.ReferenceNumber.Equals(referenceNumber) && !str.IsDeleted, token);

    public Task<NotificationType> GetNotificationTypeAsync(Guid typeId, CancellationToken token = default) =>
        Context.Set<NotificationType>()
            .SingleAsync(notificationType => notificationType.Id.Equals(typeId), cancellationToken: token);
}
