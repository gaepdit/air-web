using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Enforcement.CaseFileCommand;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AutoMapper;
using GaEpd.AppLibrary.Extensions;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Enforcement;

#pragma warning disable S107 // Methods should not have too many parameters
public sealed class CaseFileService(
    IMapper mapper,
    ICaseFileRepository caseFileRepository,
    ICaseFileManager caseFileManager,
    IComplianceWorkRepository repository,
    ICommentService<int> commentService,
    IFacilityService facilityService,
    IUserService userService,
    IAppNotificationService appNotificationService)
    : ICaseFileService
#pragma warning restore S107
{
    public async Task<CaseFileViewDto?> FindDetailedAsync(int id, CancellationToken token = default)
    {
        var caseFile = await caseFileRepository
            .FindAsync(id, includeProperties: ICaseFileRepository.IncludeDetails, token).ConfigureAwait(false);

        if (caseFile == null) return null;

        // Facility name and enforcement actions are ignored in Automapper.
        var caseFileDto = mapper.Map<CaseFileViewDto>(caseFile);

        // Facility name comes from the IAIP facility service.
        caseFileDto.FacilityName = await facilityService.GetNameAsync((FacilityId)caseFileDto.FacilityId)
            .ConfigureAwait(false);

        // Enforcement actions must be mapped individually to their respective DTOs.
        foreach (var action in caseFile.EnforcementActions)
        {
            if (action is ConsentOrder co) co.StipulatedPenalties.RemoveAll(e => e.IsDeleted);
            caseFileDto.EnforcementActions.Add(action switch
            {
                AdministrativeOrder a => mapper.Map<AoViewDto>(a),
                ConsentOrder a => mapper.Map<CoViewDto>(a),
                InformationalLetter a => mapper.Map<ResponseRequestedViewDto>(a),
                LetterOfNoncompliance a => mapper.Map<LonViewDto>(a),
                NoFurtherActionLetter a => mapper.Map<ActionViewDto>(a),
                NoticeOfViolation a => mapper.Map<NovViewDto>(a),
                NovNfaLetter a => mapper.Map<NovViewDto>(a),
                ProposedConsentOrder a => mapper.Map<ProposedCoViewDto>(a),
                _ => throw new InvalidOperationException("Unknown enforcement action type"),
            });
        }

        return caseFileDto;
    }

    public async Task<CaseFileSummaryDto?> FindSummaryAsync(int id, CancellationToken token = default)
    {
        var caseFile = mapper.Map<CaseFileSummaryDto?>(await caseFileRepository
            .FindAsync(id, includeProperties: [nameof(CaseFile.ViolationType), nameof(CaseFile.EnforcementActions)],
                token: token).ConfigureAwait(false));

        if (caseFile != null)
        {
            caseFile.FacilityName = await facilityService.GetNameAsync((FacilityId)caseFile.FacilityId)
                .ConfigureAwait(false);
        }

        return caseFile;
    }

    public async Task<CreateResult<int>> CreateAsync(CaseFileCreateDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var caseFile = await caseFileManager.CreateAsync((FacilityId)resource.FacilityId!, currentUser, token)
            .ConfigureAwait(false);

        caseFile.ResponsibleStaff = await userService.FindUserAsync(resource.ResponsibleStaffId!).ConfigureAwait(false);
        caseFile.DiscoveryDate = resource.DiscoveryDate;
        caseFile.Notes = resource.Notes ?? string.Empty;

        if (resource.EventId != null &&
            await repository.GetAsync(resource.EventId.Value, token: token).ConfigureAwait(false)
                is ComplianceEvent entry)
        {
            caseFileManager.LinkComplianceEvent(caseFile, entry, currentUser);
        }

        await caseFileRepository.InsertAsync(caseFile, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EnforcementCreated, caseFile.ResponsibleStaff, token, caseFile.Id)
            .ConfigureAwait(false);

        return CreateResult<int>.Create(caseFile.Id, notificationResult.FailureReason);
    }

    public async Task<CommandResult> UpdateAsync(int id, CaseFileUpdateDto resource,
        CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, [nameof(CaseFile.ViolationType)], token: token)
            .ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        // Update the case file properties
        caseFile.ResponsibleStaff = await userService.FindUserAsync(resource.ResponsibleStaffId!).ConfigureAwait(false);
        caseFile.DiscoveryDate = resource.DiscoveryDate;
        caseFile.Notes = resource.Notes ?? string.Empty;
        caseFile.ViolationType = await caseFileRepository.GetViolationTypeAsync(resource.ViolationTypeCode, token)
            .ConfigureAwait(false);

        caseFileManager.Update(caseFile, currentUser);
        await caseFileRepository.UpdateAsync(caseFile, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EnforcementUpdated, caseFile.ResponsibleStaff, token, id)
            .ConfigureAwait(false);

        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<IEnumerable<WorkEntrySearchResultDto>> GetLinkedEventsAsync(int id,
        CancellationToken token = default) =>
        mapper.Map<ICollection<WorkEntrySearchResultDto>>(await repository
            .GetListAsync(entry => entry.IsComplianceEvent && !entry.IsDeleted &&
                                   ((ComplianceEvent)entry).CaseFiles.Any(caseFile => caseFile.Id == id),
                WorkEntrySortBy.IdDesc.GetDescription(), token: token).ConfigureAwait(false));

    public async Task<IEnumerable<WorkEntrySearchResultDto>> GetAvailableEventsAsync(FacilityId facilityId,
        IEnumerable<WorkEntrySearchResultDto> linkedEvents, CancellationToken token = default) =>
        mapper.Map<ICollection<WorkEntrySearchResultDto>>(await repository
            .GetListAsync(entry => entry.IsComplianceEvent && !entry.IsDeleted && entry.FacilityId == facilityId,
                WorkEntrySortBy.IdDesc.GetDescription(), token: token)
            .ConfigureAwait(false)).Except(linkedEvents);

    public async Task<bool> LinkComplianceEventAsync(int id, int entryId, CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, token: token).ConfigureAwait(false);
        if (await repository.GetAsync(entryId, token: token).ConfigureAwait(false) is not ComplianceEvent entry)
            return false;
        if (entry.FacilityId != caseFile.FacilityId || caseFile.ComplianceEvents.Contains(entry))
            return false;

        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        caseFileManager.LinkComplianceEvent(caseFile, entry, currentUser);
        await caseFileRepository.UpdateAsync(caseFile, token: token).ConfigureAwait(false);
        return true;
    }

    public async Task<bool> UnLinkComplianceEventAsync(int id, int entryId, CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, [nameof(CaseFile.ComplianceEvents)], token: token)
            .ConfigureAwait(false);
        if (await repository.GetAsync(entryId, token: token).ConfigureAwait(false) is not ComplianceEvent entry)
            return false;
        if (!caseFile.ComplianceEvents.Contains(entry))
            return false;

        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        caseFileManager.UnlinkComplianceEvent(caseFile, entry, currentUser);
        await caseFileRepository.UpdateAsync(caseFile, token: token).ConfigureAwait(false);
        return true;
    }

    public Task<IEnumerable<Pollutant>> GetPollutantsAsync(int id, CancellationToken token = default) =>
        caseFileRepository.GetPollutantsAsync(id, token);

    public Task<IEnumerable<AirProgram>> GetAirProgramsAsync(int id, CancellationToken token = default) =>
        caseFileRepository.GetAirProgramsAsync(id, token);

    public async Task SaveCaseFileExtraDataAsync(int id, IEnumerable<string> pollutants,
        IEnumerable<AirProgram> airPrograms, string? violationTypeCode, CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        caseFile.ViolationType =
            await caseFileRepository.GetViolationTypeAsync(violationTypeCode, token).ConfigureAwait(false);
        caseFileManager.UpdatePollutantsAndPrograms(caseFile, pollutants, airPrograms, currentUser);
        await caseFileRepository.UpdateAsync(caseFile, token: token).ConfigureAwait(false);
    }

    public async Task<CommandResult> CloseAsync(int id, CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        caseFileManager.Close(caseFile, currentUser);
        await caseFileRepository.UpdateAsync(caseFile, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EnforcementClosed, caseFile.ResponsibleStaff, token, caseFile.Id)
            .ConfigureAwait(false);

        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> ReopenAsync(int id, CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, includeProperties: ["EnforcementActions"], token: token)
            .ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        caseFileManager.Reopen(caseFile, currentUser);
        await caseFileRepository.UpdateAsync(caseFile, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EnforcementReopened, caseFile.ResponsibleStaff, token, caseFile.Id)
            .ConfigureAwait(false);

        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> DeleteAsync(int id, CommentDto resource,
        CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        caseFileManager.Delete(caseFile, resource.Comment, currentUser);
        await caseFileRepository.UpdateAsync(caseFile, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EnforcementDeleted, caseFile.ResponsibleStaff, token, caseFile.Id)
            .ConfigureAwait(false);

        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> RestoreAsync(int id, CancellationToken token = default)
    {
        var workEntry = await caseFileRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        caseFileManager.Restore(workEntry, currentUser);
        await caseFileRepository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EnforcementRestored, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);

        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var result = await commentService.AddCommentAsync(caseFileRepository, itemId, resource, token)
            .ConfigureAwait(false);

        // FUTURE: Replace with FindAsync using a query projection.
        var caseFile = await caseFileRepository.GetAsync(resource.ItemId, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EnforcementCommentAdded, caseFile.ResponsibleStaff, token, itemId)
            .ConfigureAwait(false);

        return CreateResult<Guid>.Create(result.CommentId, notificationResult.FailureReason);
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default) =>
        commentService.DeleteCommentAsync(caseFileRepository, commentId, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        caseFileRepository.Dispose();
        caseFileManager.Dispose();
        repository.Dispose();
        userService.Dispose();
        appNotificationService.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await caseFileRepository.DisposeAsync().ConfigureAwait(false);
        await caseFileManager.DisposeAsync().ConfigureAwait(false);
        await repository.DisposeAsync().ConfigureAwait(false);
        userService.Dispose();
        await appNotificationService.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}
