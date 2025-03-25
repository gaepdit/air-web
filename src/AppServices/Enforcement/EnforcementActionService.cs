using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.AppServices.Users;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AutoMapper;

namespace AirWeb.AppServices.Enforcement;

public class EnforcementActionService(
    IEnforcementActionManager enforcementActionManager,
    IEnforcementActionRepository enforcementActionRepository,
    ICaseFileRepository caseFileRepository,
    ICaseFileManager caseFileManager,
    IMapper mapper,
    IUserService userService) : IEnforcementActionService
{
    public async Task<Guid> CreateAsync(int caseFileId, CreateEnforcementActionDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var caseFile = await caseFileRepository.GetAsync(caseFileId, token).ConfigureAwait(false);
        var enforcementAction = enforcementActionManager.Create(caseFile, resource.ActionType,
            resource.ResponseRequested, resource.Comment, currentUser);
        await enforcementActionRepository.InsertAsync(enforcementAction, token: token).ConfigureAwait(false);
        return enforcementAction.Id;
    }

    public async Task<IActionViewDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var action = await enforcementActionRepository.FindAsync(id, token).ConfigureAwait(false);
        return action is null
            ? null
            : action switch
            {
                AdministrativeOrder a => mapper.Map<AoViewDto>(a),
                OrderResolvedLetter a => mapper.Map<ActionViewDto>(a),
                ConsentOrder a => mapper.Map<CoViewDto>(a),
                InformationalLetter a => mapper.Map<ResponseRequestedViewDto>(a),
                LetterOfNoncompliance a => mapper.Map<LonViewDto>(a),
                NoFurtherActionLetter a => mapper.Map<ActionViewDto>(a),
                NoticeOfViolation a => mapper.Map<ResponseRequestedViewDto>(a),
                NovNfaLetter a => mapper.Map<ResponseRequestedViewDto>(a),
                ProposedConsentOrder a => mapper.Map<ProposedCoViewDto>(a),
                _ => throw new InvalidOperationException("Unknown enforcement action type"),
            };
    }

    public async Task AddResponse(Guid id, MaxDateAndCommentDto responseDto, CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await enforcementActionRepository.GetAsync(id, token).ConfigureAwait(false);
        enforcementActionManager.AddResponse(enforcementAction, responseDto.Date, responseDto.Comment, currentUser);
        await enforcementActionRepository.UpdateAsync(enforcementAction, token: token).ConfigureAwait(false);
    }

    public async Task IssueAsync(Guid id, MaxDateOnlyDto dateDto, CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await enforcementActionRepository.GetAsync(id, token).ConfigureAwait(false);
        enforcementActionManager.SetIssueDate(enforcementAction, dateDto.Date, currentUser);
        await enforcementActionRepository.UpdateAsync(enforcementAction, token: token).ConfigureAwait(false);
    }

    public async Task CancelAsync(Guid id, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await enforcementActionRepository.GetAsync(id, token).ConfigureAwait(false);
        enforcementActionManager.Cancel(enforcementAction, currentUser);
        await enforcementActionRepository.UpdateAsync(enforcementAction, token: token).ConfigureAwait(false);
    }

    public async Task ResolveAsync(Guid id, MaxDateAndBooleanDto resource, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await enforcementActionRepository.GetAsync(id, token).ConfigureAwait(false);
        enforcementActionManager.Resolve(enforcementAction, resource.Date, currentUser);
        await enforcementActionRepository.UpdateAsync(enforcementAction, autoSave: false, token: token)
            .ConfigureAwait(false);

        if (resource.Option)
        {
            var caseFile = await caseFileRepository.GetAsync(enforcementAction.CaseFile.Id, token)
                .ConfigureAwait(false);
            caseFileManager.Close(caseFile, currentUser);
            await caseFileRepository.UpdateAsync(caseFile, autoSave: false, token: token).ConfigureAwait(false);
        }

        // TODO: Does this also save the case file when using Entity Framework?
        await enforcementActionRepository.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await enforcementActionRepository.GetAsync(id, token).ConfigureAwait(false);
        enforcementActionManager.Delete(enforcementAction, currentUser);
        await enforcementActionRepository.UpdateAsync(enforcementAction, token: token).ConfigureAwait(false);
    }
}
