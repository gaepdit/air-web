using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.Domain.ComplianceEntities.WorkEntries;

namespace AirWeb.AppServices.Compliance.WorkEntries;

public interface IWorkEntryService : IDisposable, IAsyncDisposable
{
    // Query
    Task<IWorkEntryViewDto?> FindAsync(int id, CancellationToken token = default);
    Task<IWorkEntryCommandDto?> FindForUpdateAsync(int id, CancellationToken token = default);
    Task<WorkEntrySummaryDto?> FindSummaryAsync(int id, CancellationToken token = default);
    Task<WorkEntryType?> GetWorkEntryTypeAsync(int id, CancellationToken token = default);

    // Source test-specific
    Task<SourceTestReviewViewDto?> FindSourceTestReviewAsync(int referenceNumber, CancellationToken token = default);

    // Command
    Task<CreateResult<int>> CreateAsync(IWorkEntryCreateDto resource, CancellationToken token = default);
    Task<AppNotificationResult> UpdateAsync(int id, IWorkEntryCommandDto resource, CancellationToken token = default);
    Task<AppNotificationResult> CloseAsync(int id, CancellationToken token = default);
    Task<AppNotificationResult> ReopenAsync(int id, CancellationToken token = default);
    Task<AppNotificationResult> DeleteAsync(int id, StatusCommentDto resource, CancellationToken token = default);
    Task<AppNotificationResult> RestoreAsync(int id, CancellationToken token = default);

    // Comments
    Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource, CancellationToken token = default);
    Task DeleteCommentAsync(Guid commentId, CancellationToken token = default);
}
