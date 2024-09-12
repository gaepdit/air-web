using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;

namespace AirWeb.AppServices.Compliance.WorkEntries;

public interface IWorkEntryService : IDisposable, IAsyncDisposable
{
    // Query
    Task<IWorkEntryViewDto?> FindAsync(int id, CancellationToken token = default);
    Task<IWorkEntryUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default);
    Task<WorkEntrySummaryDto?> FindSummaryAsync(int id, CancellationToken token = default);

    // Command
    Task<CreateResult<int>> CreateAsync(IWorkEntryCreateDto resource, CancellationToken token = default);
    Task<AppNotificationResult> UpdateAsync(int id, IWorkEntryUpdateDto resource, CancellationToken token = default);
    Task<AddCommentResult> AddCommentAsync(int id, CommentAddDto<int> resource, CancellationToken token = default);
    Task<AppNotificationResult> CloseAsync(int id, CancellationToken token = default);
    Task<AppNotificationResult> ReopenAsync(int id, CancellationToken token = default);
    Task<AppNotificationResult> DeleteAsync(int id, StatusCommentDto resource, CancellationToken token = default);
    Task<AppNotificationResult> RestoreAsync(int id, CancellationToken token = default);
}
