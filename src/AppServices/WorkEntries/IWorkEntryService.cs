using AirWeb.AppServices.CommonDto;
using AirWeb.AppServices.Notifications;
using AirWeb.AppServices.WorkEntries.CommandDto;
using AirWeb.AppServices.WorkEntries.QueryDto;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.AppServices.WorkEntries;

public interface IWorkEntryService : IDisposable, IAsyncDisposable
{
    Task<WorkEntryViewDto?> FindAsync(Guid id, bool includeDeletedActions = false, CancellationToken token = default);

    Task<WorkEntryUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);

    Task<IPaginatedResult<WorkEntrySearchResultDto>> SearchAsync(WorkEntrySearchDto spec, PaginatedRequest paging,
        CancellationToken token = default);

    Task<WorkEntryCreateResult> CreateAsync(WorkEntryCreateDto resource, CancellationToken token = default);

    Task UpdateAsync(Guid id, WorkEntryUpdateDto resource, CancellationToken token = default);

    Task CloseAsync(ChangeEntityStatusDto<Guid> resource, CancellationToken token = default);

    Task<NotificationResult> ReopenAsync(ChangeEntityStatusDto<Guid> resource, CancellationToken token = default);

    Task DeleteAsync(ChangeEntityStatusDto<Guid> resource, CancellationToken token = default);

    Task RestoreAsync(ChangeEntityStatusDto<Guid> resource, CancellationToken token = default);
}
