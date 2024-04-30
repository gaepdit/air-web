using MyApp.AppServices.EntryActions.Dto;

namespace MyApp.AppServices.EntryActions;

public interface IEntryActionService : IDisposable, IAsyncDisposable
{
    Task<Guid> CreateAsync(EntryActionCreateDto resource, CancellationToken token = default);
    Task<EntryActionViewDto?> FindAsync(Guid id, CancellationToken token = default);
    Task<EntryActionUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateAsync(Guid id, EntryActionUpdateDto resource, CancellationToken token = default);
    Task DeleteAsync(Guid entryActionId, CancellationToken token = default);
    Task RestoreAsync(Guid entryActionId, CancellationToken token = default);
}
