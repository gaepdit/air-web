using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.Command;
using AirWeb.AppServices.Enforcement.Query;

namespace AirWeb.AppServices.Enforcement;

public interface IEnforcementService
{
    // Query
    Task<CaseFileViewDto?> FindDetailedCaseFileAsync(int id, CancellationToken token = default);
    Task<CaseFileSummaryDto?> FindCaseFileSummaryAsync(int id, CancellationToken token = default);

    // Command
    Task<AppNotificationResult> UpdateCaseFileAsync(int id, CaseFileUpdateDto resource, CancellationToken token);

    // Comments
    Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource, CancellationToken token = default);
    Task DeleteCommentAsync(Guid commentId, CancellationToken token = default);
}
