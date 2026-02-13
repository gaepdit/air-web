using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Core.EntityServices.Comments;
using AirWeb.AppServices.Enforcement.CaseFileCommand;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Enforcement;

public interface ICaseFileService : IDisposable, IAsyncDisposable
{
    // Query
    Task<CaseFileViewDto?> FindDetailedAsync(int id, CancellationToken token = default);
    Task<CaseFileSummaryDto?> FindSummaryAsync(int id, CancellationToken token = default);

    // Case File commands
    Task<CreateResult<int>> CreateAsync(CaseFileCreateDto resource, CancellationToken token = default);
    Task<CommandResult> UpdateAsync(int id, CaseFileUpdateDto resource, CancellationToken token = default);

    // Case File Compliance Event linkages
    Task<IEnumerable<ComplianceWorkSearchResultDto>> GetLinkedEventsAsync(int id, CancellationToken token = default);

    Task<IEnumerable<ComplianceWorkSearchResultDto>> GetAvailableEventsAsync(FacilityId facilityId,
        IEnumerable<ComplianceWorkSearchResultDto> linkedEvents, CancellationToken token = default);

    Task<bool> LinkComplianceEventAsync(int caseFileId, int eventId, CancellationToken token = default);

    Task<bool> UnLinkComplianceEventAsync(int caseFileId, int eventId, bool autoSave = true,
        CancellationToken token = default);

    // Pollutants & Air Programs
    Task<IEnumerable<Pollutant>> GetPollutantsAsync(int id, CancellationToken token = default);
    Task<IEnumerable<AirProgram>> GetAirProgramsAsync(int id, CancellationToken token = default);

    Task SaveCaseFileExtraDataAsync(int id, IEnumerable<string> pollutants, IEnumerable<AirProgram> airPrograms,
        string? violationTypeCode, CancellationToken token = default);

    // Case File workflow
    Task<CommandResult> CloseAsync(int id, CancellationToken token = default);
    Task<CommandResult> ReopenAsync(int id, CancellationToken token = default);
    Task<CommandResult> DeleteAsync(int id, CommentDto resource, CancellationToken token = default);
    Task<CommandResult> RestoreAsync(int id, CancellationToken token = default);

    // Comments
    Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default);

    Task DeleteCommentAsync(Guid commentId, CancellationToken token = default);
}
