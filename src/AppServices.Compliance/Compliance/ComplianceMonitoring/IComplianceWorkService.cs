using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Query;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.SourceTestReviews;
using AirWeb.AppServices.Core.CommonDtos;
using AirWeb.AppServices.Core.EntityServices.Comments;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;

public interface IComplianceWorkService : IDisposable, IAsyncDisposable
{
    // Query
    Task<IComplianceWorkViewDto?> FindAsync(int id, bool includeComments, CancellationToken token = default);
    Task<ComplianceWorkSummaryDto?> FindSummaryAsync(int id, CancellationToken token = default);
    Task<ComplianceWorkType?> GetComplianceWorkTypeAsync(int id, CancellationToken token = default);

    // Enforcement Cases
    Task<IEnumerable<int>> GetCaseFileIdsAsync(int id, CancellationToken token = default);

    // Source test-specific
    Task<bool> SourceTestReviewExistsAsync(int referenceNumber, CancellationToken token = default);
    Task<SourceTestReviewViewDto?> FindSourceTestReviewAsync(int referenceNumber, CancellationToken token = default);

    // Command
    Task<CreateResult<int>> CreateAsync(IComplianceWorkCreateDto resource, CancellationToken token = default);
    Task<CommandResult> UpdateAsync(int id, IComplianceWorkCommandDto resource, CancellationToken token = default);
    Task<CommandResult> CloseAsync(int id, CancellationToken token = default);
    Task<CommandResult> ReopenAsync(int id, CancellationToken token = default);
    Task<CommandResult> DeleteAsync(int id, CommentDto resource, CancellationToken token = default);
    Task<CommandResult> RestoreAsync(int id, CancellationToken token = default);

    // Comments
    Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default);

    Task DeleteCommentAsync(Guid commentId, CancellationToken token = default);
}
