using AirWeb.Domain.Entities.EntryActions;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.TestData;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalWorkEntryRepository(IEntryActionRepository entryActionRepository)
    : BaseRepository<WorkEntry, int>(WorkEntryData.GetData), IWorkEntryRepository
{
    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Select(e => e.Id).Max() + 1;

    public async Task<WorkEntry?> FindIncludeAllAsync(int id, CancellationToken token = default) =>
        await GetWorkEntryDetailsAsync(await FindAsync(id, token).ConfigureAwait(false), token)
            .ConfigureAwait(false);

    private async Task<WorkEntry?> GetWorkEntryDetailsAsync(WorkEntry? workEntry, CancellationToken token)
    {
        if (workEntry is null) return null;

        workEntry.EntryActions.Clear();
        workEntry.EntryActions.AddRange((await entryActionRepository
                .GetListAsync(action => action.WorkEntry.Id == workEntry.Id && !action.IsDeleted, token)
                .ConfigureAwait(false))
            .OrderByDescending(action => action.ActionDate)
            .ThenBy(action => action.Id));

        return workEntry;
    }
}
