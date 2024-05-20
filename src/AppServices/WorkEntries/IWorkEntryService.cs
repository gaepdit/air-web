using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Notifications;
using AirWeb.AppServices.WorkEntries.CommandDto;
using AirWeb.AppServices.WorkEntries.QueryDto;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.AppServices.WorkEntries;

public interface IWorkEntryService : IDisposable, IAsyncDisposable
{
    Task<WorkEntryViewDto?> FindAsync(int id, bool includeDeletedActions = false, CancellationToken token = default);

    Task<WorkEntryUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default);

    Task<IPaginatedResult<WorkEntrySearchResultDto>> SearchAsync(WorkEntrySearchDto spec, PaginatedRequest paging,
        CancellationToken token = default);

    Task<WorkEntryCreateResult> CreateAsync(WorkEntryCreateDto resource, CancellationToken token = default);

    Task UpdateAsync(int id, WorkEntryUpdateDto resource, CancellationToken token = default);

    Task CloseAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);

    Task<NotificationResult> ReopenAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);

    Task DeleteAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);

    Task RestoreAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);
}
