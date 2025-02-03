using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.CaseFiles;
using AirWeb.AppServices.Enforcement.Command;

namespace AirWeb.AppServices.Enforcement;

public interface IEnforcementService
{
    // FUTURE: Replace with search.
    Task<IReadOnlyCollection<CaseFileSummaryDto>> GetListAsync(CancellationToken token = default);

    // Query
    Task<CaseFileViewDto?> FindDetailedCaseFileAsync(int id, CancellationToken token = default);
    Task<CaseFileSummaryDto?> FindCaseFileSummaryAsync(int id, CancellationToken token = default);

    // Command
    Task<CreateResult<int>> CreateCaseFileAsync(CaseFileCreateDto resource, CancellationToken token = default);

    Task<AppNotificationResult> UpdateCaseFileAsync(int id, CaseFileUpdateDto resource,
        CancellationToken token = default);

    // Comments
    Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource, CancellationToken token = default);
    Task DeleteCommentAsync(Guid commentId, CancellationToken token = default);
}
