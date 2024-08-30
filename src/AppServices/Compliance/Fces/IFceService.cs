using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;

namespace AirWeb.AppServices.Compliance.Fces;

public interface IFceService : IDisposable, IAsyncDisposable
{
    // Query
    Task<FceViewDto?> FindAsync(int id, CancellationToken token = default);
    Task<FceUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default);

    // Command
    Task<CreateResult<int>> CreateAsync(FceCreateDto resource, CancellationToken token = default);
    Task<AppNotificationResult> UpdateAsync(int id, FceUpdateDto resource, CancellationToken token = default);
    Task<AddCommentResult> AddCommentAsync(int id, CommentAddDto<int> resource, CancellationToken token = default);
    Task<AppNotificationResult> DeleteAsync(int id, StatusCommentDto resource, CancellationToken token = default);
    Task<AppNotificationResult> RestoreAsync(int id, CancellationToken token = default);
    Task<bool> ExistsAsync(FceRestoreDto resource, CancellationToken token = default);
}
