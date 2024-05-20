using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Notifications;

namespace AirWeb.AppServices.Fces;

public interface IFceService : IDisposable, IAsyncDisposable
{
    // Query
    Task<FceViewDto?> FindAsync(int id, CancellationToken token = default);
    Task<FceUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default);

    // Command
    Task<CreateResultDto<int>> CreateAsync(FceCreateDto resource, CancellationToken token = default);
    Task<NotificationResult> UpdateAsync(int id, FceUpdateDto resource, CancellationToken token = default);
    Task<NotificationResult> AddCommentAsync(int id, AddCommentDto<int> resource, CancellationToken token = default);
    Task<NotificationResult> DeleteAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);
    Task<NotificationResult> RestoreAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);
}
