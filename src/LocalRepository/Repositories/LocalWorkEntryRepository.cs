using AirWeb.Domain.Entities.EntryActions;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.TestData;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalWorkEntryRepository(IEntryActionRepository entryActionRepository)
    : BaseRepository<WorkEntry>(WorkEntryData.GetData), IWorkEntryRepository
{
    public async Task<WorkEntry?> FindIncludeAllAsync(Guid id, bool includeDeletedActions = false,
        CancellationToken token = default) =>
        await GetWorkEntryDetailsAsync(await FindAsync(id, token).ConfigureAwait(false), includeDeletedActions, token)
            .ConfigureAwait(false);

    private async Task<WorkEntry?> GetWorkEntryDetailsAsync(WorkEntry? workEntry, bool includeDeletedActions,
        CancellationToken token)
    {
        if (workEntry is null) return null;

        workEntry.EntryActions.Clear();
        workEntry.EntryActions.AddRange((await entryActionRepository
                .GetListAsync(action => action.WorkEntry.Id == workEntry.Id &&
                                        (!action.IsDeleted || includeDeletedActions), token)
                .ConfigureAwait(false))
            .OrderByDescending(action => action.ActionDate)
            .ThenBy(action => action.Id));

        return workEntry;
    }
}
