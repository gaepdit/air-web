using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Notifications;
using AirWeb.AppServices.WorkEntries.BaseWorkEntryDto;
using AirWeb.AppServices.WorkEntries.Search;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.AppServices.WorkEntries;

public interface IWorkEntryService : IDisposable, IAsyncDisposable
{
    // Query
    Task<IWorkEntryViewDto?> FindAsync(int id, CancellationToken token = default);
    Task<IWorkEntryUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default);

    Task<IPaginatedResult<WorkEntrySearchResultDto>> SearchAsync(WorkEntrySearchDto spec, PaginatedRequest paging,
        CancellationToken token = default);

    // Command
    Task<CreateResultDto<int>> CreateAsync(IWorkEntryCreateDto resource, CancellationToken token = default);
    Task<NotificationResult> UpdateAsync(int id, IWorkEntryUpdateDto resource, CancellationToken token = default);
    Task<NotificationResult> AddCommentAsync(int id, AddCommentDto<int> resource, CancellationToken token = default);
    Task<NotificationResult> CloseAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);
    Task<NotificationResult> ReopenAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);
    Task<NotificationResult> DeleteAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);
    Task<NotificationResult> RestoreAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);
}
