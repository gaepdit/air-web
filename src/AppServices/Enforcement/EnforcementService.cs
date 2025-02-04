using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.CaseFiles;
using AirWeb.AppServices.Enforcement.Command;
using AirWeb.AppServices.Enforcement.EnforcementActions;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities;
using AirWeb.Domain.EnforcementEntities.Actions;
using AutoMapper;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Enforcement;

public class EnforcementService(
    IMapper mapper,
    ICaseFileRepository caseFileRepository,
    ICaseFileManager caseFileManager,
    IWorkEntryRepository workEntryRepository,
    ICommentService<int> commentService,
    IFacilityService facilityService,
    IUserService userService,
    IAppNotificationService appNotificationService)
    : IEnforcementService
{
    public async Task<IReadOnlyCollection<CaseFileSummaryDto>> GetListAsync(CancellationToken token = default) =>
        mapper.Map<IEnumerable<CaseFileSummaryDto>>(await caseFileRepository.GetListAsync("Id", token)
            .ConfigureAwait(false)).ToList();

    public async Task<CaseFileViewDto?> FindDetailedCaseFileAsync(int id, CancellationToken token = default)
    {
        var caseFile = await caseFileRepository
            .FindAsync(id, includeProperties: ICaseFileRepository.IncludeEnforcement, token).ConfigureAwait(false);

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
                AoResolvedLetter a => mapper.Map<ActionViewDto>(a),
                CoResolvedLetter a => mapper.Map<ActionViewDto>(a),
                ConsentOrder a => mapper.Map<CoViewDto>(a),
                EnforcementLetter a => mapper.Map<ResponseRequestedViewDto>(a),
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

    public async Task<CaseFileSummaryDto?> FindCaseFileSummaryAsync(int id, CancellationToken token = default)
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

    public async Task<CreateResult<int>> CreateCaseFileAsync(CaseFileCreateDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var caseFile = await caseFileManager
            .CreateCaseFileAsync((FacilityId)resource.FacilityId!, currentUser, token).ConfigureAwait(false);

        caseFile.ResponsibleStaff = await userService.FindUserAsync(resource.ResponsibleStaffId).ConfigureAwait(false);
        caseFile.DiscoveryDate = resource.DiscoveryDate;
        caseFile.Notes = resource.Notes ?? string.Empty;

        if (resource.EventId != null &&
            await workEntryRepository.FindAsync(resource.EventId.Value, token).ConfigureAwait(false) is ComplianceEvent
                complianceEvent)
        {
            caseFile.ComplianceEvents.Add(complianceEvent);
        }

        await caseFileRepository.InsertAsync(caseFile, token: token).ConfigureAwait(false);

        return new CreateResult<int>(caseFile.Id, await appNotificationService
            .SendNotificationAsync(Template.EnforcementCreated, caseFile.ResponsibleStaff, token, caseFile.Id)
            .ConfigureAwait(false));
    }

    public async Task<AppNotificationResult> UpdateCaseFileAsync(int id, CaseFileUpdateDto resource,
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

    public async Task<AppNotificationResult> CloseCaseFileAsync(int id, CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        caseFileManager.CloseCaseFile(caseFile, currentUser);
        await caseFileRepository.UpdateAsync(caseFile, autoSave: true, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EnforcementClosed, caseFile.ResponsibleStaff, token, caseFile.Id)
            .ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> ReopenCaseFileAsync(int id, CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        caseFileManager.ReopenCaseFile(caseFile, currentUser);
        await caseFileRepository.UpdateAsync(caseFile, autoSave: true, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EnforcementReopened, caseFile.ResponsibleStaff, token, caseFile.Id)
            .ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> DeleteCaseFileAsync(int id, StatusCommentDto resource,
        CancellationToken token = default)
    {
        var caseFile = await caseFileRepository.GetAsync(id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        caseFileManager.DeleteCaseFile(caseFile, resource.Comment, currentUser);
        await caseFileRepository.UpdateAsync(caseFile, autoSave: true, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EnforcementDeleted, caseFile.ResponsibleStaff, token, caseFile.Id)
            .ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> RestoreCaseFileAsync(int id, CancellationToken token = default)
    {
        var workEntry = await caseFileRepository.GetAsync(id, token).ConfigureAwait(false);
        caseFileManager.RestoreCaseFile(workEntry);
        await caseFileRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EnforcementRestored, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
    }

    public async Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var result = await commentService.AddCommentAsync(caseFileRepository, itemId, resource, token)
            .ConfigureAwait(false);

        var caseFile = await caseFileRepository.GetAsync(resource.ItemId, token).ConfigureAwait(false);

        return new CreateResult<Guid>(result.CommentId, await appNotificationService
            .SendNotificationAsync(Template.EnforcementCommentAdded, caseFile.ResponsibleStaff, token, itemId,
                resource.Comment, result.CommentUser?.FullName).ConfigureAwait(false));
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default) =>
        commentService.DeleteCommentAsync(caseFileRepository, commentId, token);
}
