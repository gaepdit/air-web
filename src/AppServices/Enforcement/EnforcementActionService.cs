using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.AppServices.IdentityServices;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AutoMapper;
using GaEpd.GuardClauses;

namespace AirWeb.AppServices.Enforcement;

public class EnforcementActionService(
    IEnforcementActionManager actionManager,
    IEnforcementActionRepository actionRepository,
    ICaseFileRepository caseFileRepository,
    ICaseFileManager caseFileManager,
    IMapper mapper,
    IUserService userService) : IEnforcementActionService
{
    public async Task<Guid> CreateAsync(int caseFileId, EnforcementActionCreateDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var caseFile = await caseFileRepository.GetAsync(caseFileId, token: token).ConfigureAwait(false);
        var enforcementAction = actionManager.Create(caseFile, resource.ActionType, currentUser);

        enforcementAction.Notes = resource.Comment;
        if (enforcementAction is IResponseRequested responseRequestedAction)
            responseRequestedAction.ResponseRequested = resource.ResponseRequested;

        await actionRepository.InsertAsync(enforcementAction, token: token).ConfigureAwait(false);
        return enforcementAction.Id;
    }

    public async Task<Guid> CreateAsync(int caseFileId, ConsentOrderCommandDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var caseFile = await caseFileRepository.GetAsync(caseFileId, token: token).ConfigureAwait(false);
        var enforcementAction = actionManager.Create(caseFile, EnforcementActionType.ConsentOrder, currentUser);

        mapper.Map(resource, enforcementAction);

        await actionRepository.InsertAsync(enforcementAction, token: token).ConfigureAwait(false);
        return enforcementAction.Id;
    }

    public async Task<Guid> CreateAsync(int caseFileId, AdministrativeOrderCommandDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var caseFile = await caseFileRepository.GetAsync(caseFileId, token: token).ConfigureAwait(false);
        var enforcementAction = actionManager.Create(caseFile, EnforcementActionType.AdministrativeOrder, currentUser);

        mapper.Map(resource, enforcementAction);

        await actionRepository.InsertAsync(enforcementAction, token: token).ConfigureAwait(false);
        return enforcementAction.Id;
    }

    public async Task<IActionViewDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var action = await actionRepository.FindAsync(id, token: token).ConfigureAwait(false);
        return action is null
            ? null
            : action switch
            {
                AdministrativeOrder a => mapper.Map<AoViewDto>(a),
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

    public async Task<EnforcementActionType?> GetEnforcementActionType(Guid id, CancellationToken token = default) =>
        await actionRepository.GetEnforcementActionType(id, token).ConfigureAwait(false);

    public async Task<CoViewDto> GetConsentOrderAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<CoViewDto>(await actionRepository.GetConsentOrder(id, token: token).ConfigureAwait(false));

    public async Task UpdateAsync(Guid id, EnforcementActionEditDto resource, CancellationToken token = default)
    {
        var entity = await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        entity.Notes = resource.Comment;
        if (entity is IResponseRequested responseRequested)
            responseRequested.ResponseRequested = resource.ResponseRequested;
        await FinishUpdateAsync(entity, resource.IssueDate, token).ConfigureAwait(false);
    }

    public async Task UpdateAsync(Guid id, ConsentOrderCommandDto resource, CancellationToken token = default)
    {
        var entity = (ConsentOrder)await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        mapper.Map(resource, entity);
        await FinishUpdateAsync(entity, resource.IssueDate, token).ConfigureAwait(false);
    }

    public async Task UpdateAsync(Guid id, AdministrativeOrderCommandDto resource, CancellationToken token = default)
    {
        var entity = (AdministrativeOrder)await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        mapper.Map(resource, entity);
        await FinishUpdateAsync(entity, resource.IssueDate, token).ConfigureAwait(false);
    }

    private async Task FinishUpdateAsync(EnforcementAction entity, DateOnly? issueDate, CancellationToken token)
    {
        actionManager.SetIssueDate(entity, issueDate, await userService.GetCurrentUserAsync().ConfigureAwait(false));
        entity.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await actionRepository.UpdateAsync(entity, token: token).ConfigureAwait(false);
    }

    public async Task AddResponse(Guid id, MaxDateAndCommentDto resource, CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        actionManager.AddResponse(enforcementAction, resource.Date, resource.Comment, currentUser);
        await actionRepository.UpdateAsync(enforcementAction, token: token).ConfigureAwait(false);
    }

    public async Task<bool> IssueAsync(Guid id, MaxDateAndBooleanDto resource, CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        actionManager.SetIssueDate(enforcementAction, resource.Date, currentUser);
        await actionRepository.UpdateAsync(enforcementAction, autoSave: false, token: token)
            .ConfigureAwait(false);

        // TODO: Move business logic to Enforcement Action Manager.
        var caseFileClosed = false;
        if (resource.Option &&
            enforcementAction.ActionType is EnforcementActionType.NovNfaLetter
                or EnforcementActionType.NoFurtherActionLetter)
        {
            var caseFile = await caseFileRepository.GetAsync(enforcementAction.CaseFile.Id, token: token)
                .ConfigureAwait(false);
            if (!caseFile.MissingPollutantsOrPrograms)
            {
                caseFileManager.Close(caseFile, currentUser);
                await caseFileRepository.UpdateAsync(caseFile, autoSave: false, token: token).ConfigureAwait(false);
                caseFileClosed = true;
            }
        }

        // TODO: Does this also save the case file when using Entity Framework?
        await actionRepository.SaveChangesAsync(token).ConfigureAwait(false);
        return caseFileClosed;
    }

    public async Task CancelAsync(Guid id, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        actionManager.Cancel(enforcementAction, currentUser);
        await actionRepository.UpdateAsync(enforcementAction, token: token).ConfigureAwait(false);
    }

    public async Task ExecuteOrderAsync(Guid id, MaxDateOnlyDto resource, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        actionManager.ExecuteOrder((IFormalEnforcementAction)enforcementAction, resource.Date, currentUser);
        await actionRepository.UpdateAsync(enforcementAction, token: token).ConfigureAwait(false);
    }

    public async Task AppealOrderAsync(Guid id, MaxDateOnlyDto resource, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await actionRepository.GetAsync(id, token: token)
            .ConfigureAwait(false);
        actionManager.AppealOrder((AdministrativeOrder)enforcementAction, resource.Date, currentUser);
        await actionRepository.UpdateAsync(enforcementAction, token: token).ConfigureAwait(false);
    }

    public async Task<bool> ResolveAsync(Guid id, MaxDateAndBooleanDto resource, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        actionManager.Resolve((IResolvable)enforcementAction, resource.Date, currentUser);
        await actionRepository.UpdateAsync(enforcementAction, autoSave: false, token: token)
            .ConfigureAwait(false);

        var caseFileClosed = false;
        if (resource.Option)
        {
            var caseFile = await caseFileRepository.GetAsync(enforcementAction.CaseFile.Id, token: token)
                .ConfigureAwait(false);
            if (!caseFile.IsClosed)
            {
                caseFileManager.Close(caseFile, currentUser);
                await caseFileRepository.UpdateAsync(caseFile, autoSave: false, token: token).ConfigureAwait(false);
                caseFileClosed = true;
            }
        }

        // TODO: Does this also save the case file when using Entity Framework?
        await actionRepository.SaveChangesAsync(token).ConfigureAwait(false);

        return caseFileClosed;
    }

    public async Task DeleteAsync(Guid id, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        actionManager.Delete(enforcementAction, currentUser);
        await actionRepository.UpdateAsync(enforcementAction, token: token).ConfigureAwait(false);
    }

    public async Task AddStipulatedPenalty(Guid id, StipulatedPenaltyAddDto resource, CancellationToken token)
    {
        Guard.NotNull(resource.ReceivedDate);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var consentOrder = (ConsentOrder)await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var penalty = actionManager.AddStipulatedPenalty(consentOrder, resource.Amount,
            resource.ReceivedDate!.Value, currentUser);
        penalty.Notes = resource.Notes;
        await actionRepository.UpdateAsync(consentOrder, token: token).ConfigureAwait(false);
    }

    public async Task DeletedStipulatedPenalty(Guid id, Guid stipulatedPenaltyId, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var consentOrder = (ConsentOrder)await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var penalty = consentOrder.StipulatedPenalties.SingleOrDefault(penalty => penalty.Id == stipulatedPenaltyId);
        if (penalty == null) return;
        actionManager.DeleteStipulatedPenalty(penalty, currentUser);
        await actionRepository.UpdateAsync(consentOrder, token: token).ConfigureAwait(false);
    }
}
