using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.EfRepository.DbContext;
using System.Linq.Expressions;

namespace AirWeb.EfRepository.Repositories;

public sealed class WorkEntryRepository(AppDbContext context)
    : BaseRepository<BaseWorkEntry, int, AppDbContext>(context), IWorkEntryRepository
{
    // Entity Framework will set the ID automatically.
    public int? GetNextId() => null;

    public Task<WorkEntryType> GetWorkEntryTypeAsync(int id, CancellationToken token = default) =>
        Context.Set<BaseWorkEntry>().AsNoTracking()
            .Where(entry => entry.Id.Equals(id)).Select(entry => entry.WorkEntryType).SingleAsync(token);

    public Task<ComplianceEventType> GetComplianceEventTypeAsync(int id, CancellationToken token = default) =>
        Context.Set<BaseComplianceEvent>().AsNoTracking()
            .Where(complianceEvent => complianceEvent.Id.Equals(id))
            .Select(complianceEvent => complianceEvent.ComplianceEventType).SingleOrDefaultAsync(token);

    public Task<TEntry?> FindAsync<TEntry>(int id, CancellationToken token = default) where TEntry : BaseWorkEntry =>
        Context.Set<TEntry>().AsNoTracking().SingleOrDefaultAsync(entry => entry.Id.Equals(id), token);

    public Task<BaseWorkEntry?> FindAsync(Expression<Func<BaseWorkEntry, bool>> predicate,
        string[] includeProperties, CancellationToken token = default) =>
        includeProperties
            .Aggregate(Context.Set<BaseWorkEntry>().AsNoTracking(),
                (queryable, includeProperty) => queryable.Include(includeProperty))
            .SingleOrDefaultAsync(predicate, token);
}
