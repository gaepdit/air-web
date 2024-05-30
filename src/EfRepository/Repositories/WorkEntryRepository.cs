using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.ValueObjects;
using AirWeb.EfRepository.DbContext;
using GaEpd.AppLibrary.Domain.Repositories;
using System.Linq.Expressions;

namespace AirWeb.EfRepository.Repositories;

public sealed class WorkEntryRepository(AppDbContext context)
    : BaseRepository<BaseWorkEntry, int, AppDbContext>(context), IWorkEntryRepository
{
    // Entity Framework will set the ID automatically.
    public int? GetNextId() => null;

    public async Task<BaseWorkEntry> GetAsync(int id, string[] includeProperties, CancellationToken token = default) =>
        await includeProperties.Aggregate(Context.Set<BaseWorkEntry>().AsNoTracking(),
                (queryable, includeProperty) => queryable.Include(includeProperty))
            .SingleOrDefaultAsync(entry => entry.Id.Equals(id), token).ConfigureAwait(false)
        ?? throw new EntityNotFoundException<BaseWorkEntry>(id);

    public Task<BaseWorkEntry?> FindAsync(Expression<Func<BaseWorkEntry, bool>> predicate,
        string[] includeProperties, CancellationToken token = default) =>
        includeProperties.Aggregate(Context.Set<BaseWorkEntry>().AsNoTracking(),
                (queryable, includeProperty) => queryable.Include(includeProperty))
            .SingleOrDefaultAsync(predicate, token);

    public Task<TEntry?> FindAsync<TEntry>(int id, CancellationToken token = default) where TEntry : BaseWorkEntry =>
        Context.Set<TEntry>().AsNoTracking().SingleOrDefaultAsync(entry => entry.Id.Equals(id), token);

    public Task<WorkEntryType> GetWorkEntryTypeAsync(int id, CancellationToken token = default) =>
        Context.Set<BaseWorkEntry>().AsNoTracking()
            .Where(entry => entry.Id.Equals(id)).Select(entry => entry.WorkEntryType).SingleAsync(token);

    public Task<ComplianceEventType> GetComplianceEventTypeAsync(int id, CancellationToken token = default) =>
        Context.Set<BaseComplianceEvent>().AsNoTracking()
            .Where(complianceEvent => complianceEvent.Id.Equals(id))
            .Select(complianceEvent => complianceEvent.ComplianceEventType).SingleOrDefaultAsync(token);

    public async Task AddCommentAsync(int id, Comment comment, CancellationToken token = default)
    {
        var entry = await Context.Set<BaseWorkEntry>().AsNoTracking()
            .SingleAsync(entry => entry.Id.Equals(id), token).ConfigureAwait(false);
        entry.Comments.Add(comment);
        await UpdateAsync(entry, true, token).ConfigureAwait(false);
    }
}
