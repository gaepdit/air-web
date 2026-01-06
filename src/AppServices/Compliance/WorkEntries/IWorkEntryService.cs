using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;

namespace AirWeb.AppServices.Compliance.WorkEntries;

public interface IWorkEntryService : IDisposable, IAsyncDisposable
{
    // Query
    Task<IWorkEntryViewDto?> FindAsync(int id, bool includeComments, CancellationToken token = default);
    Task<WorkEntrySummaryDto?> FindSummaryAsync(int id, CancellationToken token = default);
    Task<ComplianceWorkType?> GetWorkEntryTypeAsync(int id, CancellationToken token = default);

    // Enforcement Cases
    Task<IEnumerable<int>> GetCaseFileIdsAsync(int id, CancellationToken token = default);

    // Source test-specific
    Task<bool> SourceTestReviewExistsAsync(int referenceNumber, CancellationToken token = default);
    Task<SourceTestReviewViewDto?> FindSourceTestReviewAsync(int referenceNumber, CancellationToken token = default);

    // Command
    Task<CreateResult<int>> CreateAsync(IWorkEntryCreateDto resource, CancellationToken token = default);
    Task<CommandResult> UpdateAsync(int id, IWorkEntryCommandDto resource, CancellationToken token = default);
    Task<CommandResult> CloseAsync(int id, CancellationToken token = default);
    Task<CommandResult> ReopenAsync(int id, CancellationToken token = default);
    Task<CommandResult> DeleteAsync(int id, CommentDto resource, CancellationToken token = default);
    Task<CommandResult> RestoreAsync(int id, CancellationToken token = default);

    // Comments
    Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default);

    Task DeleteCommentAsync(Guid commentId, CancellationToken token = default);
}
