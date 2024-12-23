using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Fces;

public interface IFceService : IDisposable, IAsyncDisposable
{
    // Query
    Task<FceViewDto?> FindAsync(int id, CancellationToken token = default);
    Task<FceSummaryDto?> FindSummaryAsync(int id, CancellationToken token = default);

    // Command
    Task<CreateResult<int>> CreateAsync(FceCreateDto resource, CancellationToken token = default);
    Task<AppNotificationResult> UpdateAsync(int id, FceUpdateDto resource, CancellationToken token = default);
    Task<AppNotificationResult> DeleteAsync(int id, StatusCommentDto resource, CancellationToken token = default);
    Task<AppNotificationResult> RestoreAsync(int id, CancellationToken token = default);
    Task<bool> ExistsAsync(FacilityId facilityId, int year, int currentId, CancellationToken token = default);

    // Comments
    Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource, CancellationToken token = default);
    Task DeleteCommentAsync(Guid commentId, CancellationToken token = default);
}
