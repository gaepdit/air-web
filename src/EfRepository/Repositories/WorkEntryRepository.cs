using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.EfRepository.DbContext;

namespace AirWeb.EfRepository.Repositories;

public sealed class WorkEntryRepository(AppDbContext context)
    : BaseRepository<WorkEntry, int, AppDbContext>(context), IWorkEntryRepository
{
    // Entity Framework will set the ID automatically.
    public int? GetNextId() => null;

    public Task<WorkEntry?> FindIncludeAllAsync(int id, CancellationToken token = default) =>
        Context.Set<WorkEntry>().AsNoTracking()
            .Include(entry => entry.EntryActions
                .Where(action => !action.IsDeleted)
                .OrderByDescending(action => action.ActionDate)
                .ThenBy(action => action.Id)
            ).ThenInclude(action => action.DeletedBy)
            .AsSplitQuery()
            .SingleOrDefaultAsync(entry => entry.Id.Equals(id), token);
}
