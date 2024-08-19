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

    // Command
    Task<CreateResult<int>> CreateAsync(IWorkEntryCreateDto resource, CancellationToken token = default);
    Task<AppNotificationResult> UpdateAsync(int id, IWorkEntryUpdateDto resource, CancellationToken token = default);
    Task<AppNotificationResult> AddCommentAsync(int id, CommentAddDto<int> resource, CancellationToken token = default);
    Task<AppNotificationResult> CloseAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);
    Task<AppNotificationResult> ReopenAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);
    Task<AppNotificationResult> DeleteAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);
    Task<AppNotificationResult> RestoreAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);
}
