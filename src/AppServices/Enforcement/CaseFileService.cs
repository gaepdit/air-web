using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Enforcement.CaseFileCommand;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AutoMapper;
using GaEpd.AppLibrary.Extensions;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Enforcement;

public class CaseFileService(
    IMapper mapper,
    ICaseFileRepository caseFileRepository,
    ICaseFileManager caseFileManager,
    IWorkEntryRepository entryRepository,
    ICommentService<int> commentService,
    IFacilityService facilityService,
    IUserService userService,
    IAppNotificationService appNotificationService)
    : ICaseFileService
{
    public async Task<IReadOnlyCollection<CaseFileSummaryDto>> GetListAsync(CancellationToken token = default) =>
        mapper.Map<IEnumerable<CaseFileSummaryDto>>(await caseFileRepository.GetListAsync("Id", token)
            .ConfigureAwait(false)).ToList();

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
            caseFileDto.EnforcementActions.Add(action switch
            {
                AdministrativeOrder a => mapper.Map<AoViewDto>(a),
                OrderResolvedLetter a => mapper.Map<ActionViewDto>(a),
                ConsentOrder a => mapper.Map<CoViewDto>(a),
                InformationalLetter a => mapper.Map<ResponseRequestedViewDto>(a),
                LetterOfNoncompliance a => mapper.Map<ResponseRequestedViewDto>(a),
                NoFurtherActionLetter a => mapper.Map<ActionViewDto>(a),
                NoticeOfViolation a => mapper.Map<ResponseRequestedViewDto>(a),
                NovNfaLetter a => mapper.Map<ResponseRequestedViewDto>(a),
                ProposedConsentOrder a => mapper.Map<ProposedCoViewDto>(a),
                _ => throw new InvalidOperationException("Unknown enforcement action type"),
            });
        }

        return caseFileDto;
    }

    public async Task<CaseFileSummaryDto?> FindSummaryAsync(int id, CancellationToken token = default)
    {
        var caseFile = mapper.Map<CaseFileSummaryDto?>(await caseFileRepository.FindAsync(id, token)
            .ConfigureAwait(false));

        if (caseFile != null)
        {
            caseFile.FacilityName = await facilityService.GetNameAsync((FacilityId)caseFile.FacilityId)
                .ConfigureAwait(false);
        }

        return caseFile;
    }

    public async Task<NotificationResultWithId<int>> CreateAsync(CaseFileCreateDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var caseFile = await caseFileManager
            .Create((FacilityId)resource.FacilityId!, currentUser, token).ConfigureAwait(false);

        caseFile.ResponsibleStaff = await userService.FindUserAsync(resource.ResponsibleStaffId).ConfigureAwait(false);
        caseFile.DiscoveryDate = resource.DiscoveryDate;
        caseFile.Notes = resource.Notes ?? string.Empty;

        if (resource.EventId != null &&
            await entryRepository.FindAsync(resource.EventId.Value, token).ConfigureAwait(false) is ComplianceEvent
                complianceEvent)
        {
            caseFile.ComplianceEvents.Add(complianceEvent);
        }

        await caseFileRepository.InsertAsync(caseFile, token: token).ConfigureAwait(false);

        return new NotificationResultWithId<int>(caseFile.Id, await appNotificationService
            .SendNotificationAsync(Template.EnforcementCreated, caseFile.ResponsibleStaff, token, caseFile.Id)
            .ConfigureAwait(false));
    }

    public async Task<AppNotificationResult> UpdateAsync(int id, CaseFileUpdateDto resource,
        CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, token).ConfigureAwait(false);
        caseFile.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);

        // Update the case file properties
        caseFile.ResponsibleStaff = await userService.FindUserAsync(resource.ResponsibleStaffId).ConfigureAwait(false);
        caseFile.DiscoveryDate = resource.DiscoveryDate;
        caseFile.Notes = resource.Notes ?? string.Empty;

        await caseFileRepository.UpdateAsync(caseFile, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EnforcementUpdated, caseFile.ResponsibleStaff, token, id)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<WorkEntrySearchResultDto>> GetLinkedEventsAsync(int id,
        CancellationToken token = default) =>
        mapper.Map<ICollection<WorkEntrySearchResultDto>>(await entryRepository
            .GetListAsync(entry => entry.IsComplianceEvent && !entry.IsDeleted &&
                                   ((ComplianceEvent)entry).CaseFiles.Any(caseFile => caseFile.Id == id),
                SortBy.IdDesc.GetDescription(), token).ConfigureAwait(false));

    public async Task<IEnumerable<WorkEntrySearchResultDto>> GetAvailableEventsAsync(FacilityId facilityId,
        IEnumerable<WorkEntrySearchResultDto> linkedEvents, CancellationToken token = default) =>
        mapper.Map<ICollection<WorkEntrySearchResultDto>>(await entryRepository
            .GetListAsync(entry => entry.IsComplianceEvent && !entry.IsDeleted && entry.FacilityId == facilityId,
                SortBy.IdDesc.GetDescription(), token)
            .ConfigureAwait(false)).Except(linkedEvents);

    public async Task<bool> LinkComplianceEvent(int id, int entryId, CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, token).ConfigureAwait(false);
        if (await entryRepository.GetAsync(entryId, token).ConfigureAwait(false) is not ComplianceEvent entry)
            return false;
        if (entry.FacilityId != caseFile.FacilityId || caseFile.ComplianceEvents.Contains(entry))
            return false;

        caseFile.ComplianceEvents.Add(entry);
        entry.CaseFiles.Add(caseFile);
        await caseFileRepository.UpdateAsync(caseFile, token: token).ConfigureAwait(false);
        await entryRepository.UpdateAsync(entry, token: token).ConfigureAwait(false);
        return true;
    }

    public async Task<bool> UnLinkComplianceEvent(int id, int entryId, CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, token).ConfigureAwait(false);
        if (await entryRepository.GetAsync(entryId, token).ConfigureAwait(false) is not ComplianceEvent entry)
            return false;
        if (!caseFile.ComplianceEvents.Contains(entry))
            return false;

        caseFile.ComplianceEvents.Remove(entry);
        await caseFileRepository.UpdateAsync(caseFile, token: token).ConfigureAwait(false);
        entry.CaseFiles.Remove(caseFile);
        await entryRepository.UpdateAsync(entry, token: token).ConfigureAwait(false);
        return true;
    }

    public async Task<AppNotificationResult> CloseAsync(int id, CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        caseFileManager.Close(caseFile, currentUser);
        await caseFileRepository.UpdateAsync(caseFile, autoSave: true, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EnforcementClosed, caseFile.ResponsibleStaff, token, caseFile.Id)
            .ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> ReopenAsync(int id, CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        caseFileManager.Reopen(caseFile, currentUser);
        await caseFileRepository.UpdateAsync(caseFile, autoSave: true, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EnforcementReopened, caseFile.ResponsibleStaff, token, caseFile.Id)
            .ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> DeleteAsync(int id, CommentDto resource,
        CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        caseFileManager.Delete(caseFile, resource.Comment, currentUser);
        await caseFileRepository.UpdateAsync(caseFile, autoSave: true, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EnforcementDeleted, caseFile.ResponsibleStaff, token, caseFile.Id)
            .ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> RestoreAsync(int id, CancellationToken token = default)
    {
        var workEntry = await caseFileRepository.GetAsync(id, token).ConfigureAwait(false);
        caseFileManager.Restore(workEntry);
        await caseFileRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EnforcementRestored, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
    }

    public async Task<NotificationResultWithId<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var result = await commentService.AddCommentAsync(caseFileRepository, itemId, resource, token)
            .ConfigureAwait(false);

        var caseFile = await caseFileRepository.GetAsync(resource.ItemId, token).ConfigureAwait(false);

        return new NotificationResultWithId<Guid>(result.CommentId, await appNotificationService
            .SendNotificationAsync(Template.EnforcementCommentAdded, caseFile.ResponsibleStaff, token, itemId,
                resource.Comment, result.CommentUser?.FullName).ConfigureAwait(false));
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default) =>
        commentService.DeleteCommentAsync(caseFileRepository, commentId, token);
}
