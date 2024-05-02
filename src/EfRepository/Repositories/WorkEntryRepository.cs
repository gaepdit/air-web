using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.EfRepository.DbContext;

namespace AirWeb.EfRepository.Repositories;

public sealed class WorkEntryRepository(AppDbContext context)
    : BaseRepository<WorkEntry, AppDbContext>(context), IWorkEntryRepository
{
    public Task<WorkEntry?> FindIncludeAllAsync(Guid id, bool includeDeletedActions = false,
        CancellationToken token = default) =>
        Context.Set<WorkEntry>().AsNoTracking()
            .Include(entry => entry.EntryActions
                .Where(action => !action.IsDeleted || includeDeletedActions)
                .OrderByDescending(action => action.ActionDate)
                .ThenBy(action => action.Id)
            ).ThenInclude(action => action.DeletedBy)
            .AsSplitQuery()
            .SingleOrDefaultAsync(entry => entry.Id.Equals(id), token);
}
