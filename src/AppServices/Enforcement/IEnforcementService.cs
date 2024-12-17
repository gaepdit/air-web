using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.Command;
using AirWeb.AppServices.Enforcement.Query;

namespace AirWeb.AppServices.Enforcement;

public interface IEnforcementService
{
    // Query
    Task<CaseFileViewDto?> FindAsync(int id, CancellationToken token = default);

    // Command
    Task<CreateResult<int>> CreateCaseFileAsync(CaseFileCreateDto resource, CancellationToken token = default);

    // Comments
    Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource, CancellationToken token = default);
    Task DeleteCommentAsync(Guid commentId, CancellationToken token = default);
}
