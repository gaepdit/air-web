using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Enforcement.CaseFileCommand;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Enforcement;

public interface ICaseFileService
{
    // FUTURE: Replace with search.
    Task<IReadOnlyCollection<CaseFileSummaryDto>> GetListAsync(CancellationToken token = default);

    // Query
    Task<CaseFileViewDto?> FindDetailedAsync(int id, CancellationToken token = default);
    Task<CaseFileSummaryDto?> FindSummaryAsync(int id, CancellationToken token = default);

    // Case File commands
    Task<NotificationResultWithId<int>> CreateAsync(CaseFileCreateDto resource, CancellationToken token = default);

    Task<AppNotificationResult> UpdateAsync(int id, CaseFileUpdateDto resource,
        CancellationToken token = default);

    // Case File Compliance Event linkages
    Task<IEnumerable<WorkEntrySearchResultDto>> GetLinkedEventsAsync(int id, CancellationToken token = default);

    Task<IEnumerable<WorkEntrySearchResultDto>> GetAvailableEventsAsync(FacilityId facilityId,
        IEnumerable<WorkEntrySearchResultDto> linkedEvents, CancellationToken token = default);

    Task<bool> LinkComplianceEvent(int id, int entryId, CancellationToken token = default);
    Task<bool> UnLinkComplianceEvent(int id, int entryId, CancellationToken token = default);

    // Pollutants & Air Programs
    Task<IEnumerable<Pollutant>> GetPollutantsAsync(int id, CancellationToken token = default);
    Task<IEnumerable<AirProgram>> GetAirProgramsAsync(int id, CancellationToken token = default);

    Task SavePollutantsAndProgramsAsync(int id, IEnumerable<string> pollutants, IEnumerable<AirProgram> airPrograms,
        CancellationToken token = default);

    // Case File workflow
    Task<AppNotificationResult> CloseAsync(int id, CancellationToken token = default);
    Task<AppNotificationResult> ReopenAsync(int id, CancellationToken token = default);

    Task<AppNotificationResult> DeleteAsync(int id, CommentDto resource,
        CancellationToken token = default);

    Task<AppNotificationResult> RestoreAsync(int id, CancellationToken token = default);

    // Comments
    Task<NotificationResultWithId<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default);

    Task DeleteCommentAsync(Guid commentId, CancellationToken token = default);
}
