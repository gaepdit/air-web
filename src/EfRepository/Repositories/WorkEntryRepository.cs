using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.ValueObjects;
using AirWeb.EfRepository.DbContext;
using GaEpd.AppLibrary.Domain.Repositories;

namespace AirWeb.EfRepository.Repositories;

public sealed class WorkEntryRepository(AppDbContext context)
    : BaseRepository<WorkEntry, int, AppDbContext>(context), IWorkEntryRepository
{
    // Entity Framework will set the ID automatically.
    public int? GetNextId() => null;

    public async Task<WorkEntry> GetAsync(int id, string[] includeProperties, CancellationToken token = default) =>
        await includeProperties.Aggregate(Context.Set<WorkEntry>().AsNoTracking(),
                (queryable, includeProperty) => queryable.Include(includeProperty))
            .SingleOrDefaultAsync(entry => entry.Id.Equals(id), token).ConfigureAwait(false)
        ?? throw new EntityNotFoundException<WorkEntry>(id);

    public Task<WorkEntry?> FindAsync(int id, string[] includeProperties, CancellationToken token = default) =>
        includeProperties.Aggregate(Context.Set<WorkEntry>().AsNoTracking(),
                (queryable, includeProperty) => queryable.Include(includeProperty))
            .SingleOrDefaultAsync(entry => entry.Id.Equals(id), token);

    public Task<TEntry?> FindAsync<TEntry>(int id, CancellationToken token = default)
        where TEntry : WorkEntry =>
        Context.Set<TEntry>().AsNoTracking().SingleOrDefaultAsync(entry => entry.Id.Equals(id), token);

    public Task<TEntry?> FindWithCommentsAsync<TEntry>(int id, CancellationToken token = default)
        where TEntry : WorkEntry =>
        Context.Set<TEntry>().AsNoTracking()
            .Include(entry => entry.Comments)
            .SingleOrDefaultAsync(entry => entry.Id.Equals(id), token);

    public Task<WorkEntryType> GetWorkEntryTypeAsync(int id, CancellationToken token = default) =>
        Context.Set<WorkEntry>().AsNoTracking()
            .Where(entry => entry.Id.Equals(id)).Select(entry => entry.WorkEntryType).SingleAsync(token);

    public Task<ComplianceEventType> GetComplianceEventTypeAsync(int id, CancellationToken token = default) =>
        Context.Set<ComplianceEvent>().AsNoTracking()
            .Where(complianceEvent => complianceEvent.Id.Equals(id))
            .Select(complianceEvent => complianceEvent.ComplianceEventType).SingleAsync(token);

    public Task<NotificationType> GetNotificationTypeAsync(Guid typeId, CancellationToken token = default) =>
        Context.Set<NotificationType>().AsNoTracking()
            .SingleAsync(notificationType => notificationType.Id.Equals(typeId), cancellationToken: token);

    public async Task AddCommentAsync(int id, Comment comment, CancellationToken token = default)
    {
        Context.WorkEntryComments.Add(new WorkEntryComment(comment, id));
        await SaveChangesAsync(token).ConfigureAwait(false);
    }
}
