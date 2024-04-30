using AutoMapper;
using MyApp.AppServices.EntryActions.Dto;
using MyApp.AppServices.UserServices;
using MyApp.Domain.Entities.EntryActions;
using MyApp.Domain.Entities.WorkEntries;

namespace MyApp.AppServices.EntryActions;

public sealed class EntryActionService(
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IUserService userService,
    IWorkEntryRepository workEntryRepository,
    IWorkEntryManager workEntryManager,
    IEntryActionRepository entryActionRepository)
    : IEntryActionService
{
    public async Task<Guid> CreateAsync(EntryActionCreateDto resource, CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.WorkEntryId, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var entryAction = workEntryManager.CreateEntryAction(workEntry, currentUser);

        entryAction.ActionDate = resource.ActionDate!.Value;
        entryAction.Comments = resource.Comments;

        await entryActionRepository.InsertAsync(entryAction, token: token).ConfigureAwait(false);
        return entryAction.Id;
    }

    public async Task<EntryActionViewDto?> FindAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<EntryActionViewDto>(
            await entryActionRepository.FindAsync(id, token).ConfigureAwait(false));

    public async Task<EntryActionUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<EntryActionUpdateDto>(
            await entryActionRepository.FindAsync(action => action.Id == id && !action.IsDeleted, token)
                .ConfigureAwait(false));

    public async Task UpdateAsync(Guid id, EntryActionUpdateDto resource, CancellationToken token = default)
    {
        var entryAction = await entryActionRepository.GetAsync(id, token).ConfigureAwait(false);
        entryAction.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);

        entryAction.ActionDate = resource.ActionDate!.Value;
        entryAction.Comments = resource.Comments;

        await entryActionRepository.UpdateAsync(entryAction, token: token).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Guid entryActionId, CancellationToken token = default)
    {
        var entryAction = await entryActionRepository.GetAsync(entryActionId, token).ConfigureAwait(false);
        entryAction.SetDeleted((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await entryActionRepository.UpdateAsync(entryAction, token: token).ConfigureAwait(false);
    }

    public async Task RestoreAsync(Guid entryActionId, CancellationToken token = default)
    {
        var entryAction = await entryActionRepository.GetAsync(entryActionId, token).ConfigureAwait(false);
        entryAction.SetNotDeleted();
        await entryActionRepository.UpdateAsync(entryAction, token: token).ConfigureAwait(false);
    }

    public void Dispose()
    {
        workEntryRepository.Dispose();
        entryActionRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await workEntryRepository.DisposeAsync().ConfigureAwait(false);
        await entryActionRepository.DisposeAsync().ConfigureAwait(false);
    }
}
