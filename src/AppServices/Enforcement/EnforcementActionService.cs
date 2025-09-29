using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Identity;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using GaEpd.GuardClauses;
using Microsoft.Extensions.Logging;

namespace AirWeb.AppServices.Enforcement;

#pragma warning disable S107 // Methods should not have too many parameters
public sealed class EnforcementActionService(
    IEnforcementActionManager actionManager,
    IEnforcementActionRepository actionRepository,
    ICaseFileRepository caseFileRepository,
    ICaseFileManager caseFileManager,
    IMapper mapper,
    IUserService userService,
    ILogger<EnforcementActionService> logger,
    IAppNotificationService appNotificationService)
    : IEnforcementActionService
#pragma warning restore S107
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

        await appNotificationService
            .SendNotificationAsync(Template.EnforcementActionAdded, caseFile.ResponsibleStaff, token, caseFileId)
            .ConfigureAwait(false);

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

        await appNotificationService
            .SendNotificationAsync(Template.EnforcementActionAdded, caseFile.ResponsibleStaff, token, caseFileId)
            .ConfigureAwait(false);

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

        await appNotificationService
            .SendNotificationAsync(Template.EnforcementActionAdded, caseFile.ResponsibleStaff, token, caseFileId)
            .ConfigureAwait(false);

        return enforcementAction.Id;
    }

    public async Task<IActionViewDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var action = await actionRepository
            .FindAsync(id, includeProperties: [nameof(EnforcementAction.Reviews), nameof(EnforcementAction.CaseFile)],
                token: token).ConfigureAwait(false);
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
        (await actionRepository.FindAsync<ActionTypeDto>(id, mapper, token).ConfigureAwait(false))?.ActionType;

    public async Task<CoViewDto?> FindConsentOrderAsync(Guid id, CancellationToken token = default)
    {
        var coViewDto = await actionRepository.FindAsync<CoViewDto, ConsentOrder>(id, mapper, token)
            .ConfigureAwait(false);
        coViewDto?.StipulatedPenalties.RemoveAll(p => p.IsDeleted);
        return coViewDto;
    }

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
        var enforcementAction = await actionRepository.GetAsync(id, [nameof(EnforcementAction.CaseFile)], token: token)
            .ConfigureAwait(false);
        actionManager.SetIssueDate(enforcementAction, resource.Date, currentUser);
        await actionRepository.UpdateAsync(enforcementAction, autoSave: false, token: token).ConfigureAwait(false);

        // TODO: Move business logic to Enforcement Action Manager.
        var caseFileClosed = false;
        if (resource.Option &&
            enforcementAction.ActionType is EnforcementActionType.NovNfaLetter
                or EnforcementActionType.NoFurtherActionLetter)
        {
            var caseFile = enforcementAction.CaseFile;
            if (!caseFile.MissingPollutantsOrPrograms)
            {
                caseFileManager.Close(caseFile, currentUser);
                await caseFileRepository.UpdateAsync(caseFile, autoSave: false, token: token).ConfigureAwait(false);
                caseFileClosed = true;

                await appNotificationService
                    .SendNotificationAsync(Template.EnforcementClosed, caseFile.ResponsibleStaff, token, caseFile.Id)
                    .ConfigureAwait(false);
            }
        }

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
        var enforcementAction = await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        actionManager.AppealOrder((AdministrativeOrder)enforcementAction, resource.Date, currentUser);
        await actionRepository.UpdateAsync(enforcementAction, token: token).ConfigureAwait(false);
    }

    public async Task<bool> ResolveAsync(Guid id, MaxDateAndBooleanDto resource, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await actionRepository.GetAsync(id, [nameof(EnforcementAction.CaseFile)], token: token)
            .ConfigureAwait(false);
        actionManager.Resolve((IResolvable)enforcementAction, resource.Date, currentUser);
        await actionRepository.UpdateAsync(enforcementAction, autoSave: false, token: token).ConfigureAwait(false);

        // TODO: Move business logic to Enforcement Action Manager.
        var caseFileClosed = false;
        if (resource.Option)
        {
            var caseFile = enforcementAction.CaseFile;
            if (!caseFile.IsClosed)
            {
                caseFileManager.Close(caseFile, currentUser);
                await caseFileRepository.UpdateAsync(caseFile, autoSave: false, token: token).ConfigureAwait(false);
                caseFileClosed = true;

                await appNotificationService
                    .SendNotificationAsync(Template.EnforcementClosed, caseFile.ResponsibleStaff, token, caseFile.Id)
                    .ConfigureAwait(false);
            }
        }

        await actionRepository.SaveChangesAsync(token).ConfigureAwait(false);
        return caseFileClosed;
    }

    public async Task DeleteAsync(Guid id, int caseFileId, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var caseFile = await caseFileRepository.GetAsync(caseFileId, token: token).ConfigureAwait(false);
        actionManager.Delete(enforcementAction, caseFile, currentUser);
        await actionRepository.UpdateAsync(enforcementAction, token: token).ConfigureAwait(false);

        await appNotificationService
            .SendNotificationAsync(Template.EnforcementActionDeleted, caseFile.ResponsibleStaff, token, caseFile.Id)
            .ConfigureAwait(false);
    }

    public async Task AddStipulatedPenalty(Guid id, StipulatedPenaltyAddDto resource, CancellationToken token)
    {
        Guard.NotNull(resource.ReceivedDate);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var consentOrder = (ConsentOrder)await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var penalty = actionManager.AddStipulatedPenalty(consentOrder, resource.Amount, resource.ReceivedDate!.Value,
            currentUser);
        penalty.Notes = resource.Notes;
        await actionRepository.UpdateAsync(consentOrder, token: token).ConfigureAwait(false);
    }

    public async Task DeletedStipulatedPenalty(Guid id, Guid stipulatedPenaltyId, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var consentOrder = (ConsentOrder)await actionRepository
            .GetAsync(id, [nameof(ConsentOrder.StipulatedPenalties)], token: token).ConfigureAwait(false);
        var penalty = consentOrder.StipulatedPenalties.SingleOrDefault(penalty => penalty.Id == stipulatedPenaltyId);
        if (penalty == null) return;
        actionManager.DeleteStipulatedPenalty(penalty, currentUser);
        await actionRepository.UpdateAsync(consentOrder, token: token).ConfigureAwait(false);
    }

    public async Task RequestReviewAsync(Guid id, EnforcementActionRequestReviewDto resource, CancellationToken token)
    {
        var action = await actionRepository.GetAsync(id, [nameof(EnforcementAction.CaseFile)], token: token)
            .ConfigureAwait(false);
        var reviewer = await userService.GetUserAsync(resource.RequestedOfId!).ConfigureAwait(false);

        if (!await userService.UserIsInRoleAsync(reviewer, RoleName.EnforcementManager).ConfigureAwait(false))
        {
            logger.LogError(
                "User {UserId} does not have the Enforcement Manager role and cannot review action {ActionId}.",
                reviewer.Id, action.Id);
            return;
        }

        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        actionManager.RequestReview(action, reviewer, currentUser!);
        await actionRepository.UpdateAsync(action, token: token).ConfigureAwait(false);

        await appNotificationService
            .SendNotificationAsync(Template.EnforcementActionReviewRequested, reviewer, token, action.CaseFile.Id)
            .ConfigureAwait(false);
    }

    public async Task SubmitReviewAsync(Guid id, EnforcementActionSubmitReviewDto resource, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var action = await actionRepository
            .GetAsync(id, includeProperties: [nameof(EnforcementAction.Reviews), nameof(EnforcementAction.CaseFile)],
                token: token).ConfigureAwait(false);

        var nextReviewer = resource.RequestedOfId is null
            ? null
            : await userService.FindUserAsync(resource.RequestedOfId).ConfigureAwait(false);

        actionManager.SubmitReview(action, resource.Result!.Value, resource.Comment, currentUser!, nextReviewer);
        await actionRepository.UpdateAsync(action, token: token).ConfigureAwait(false);

        await appNotificationService.SendNotificationAsync(Template.EnforcementActionReviewCompleted,
            action.CaseFile.ResponsibleStaff, token, action.CaseFile.Id).ConfigureAwait(false);

        if (nextReviewer is not null)
            await appNotificationService.SendNotificationAsync(Template.EnforcementActionReviewRequested, nextReviewer,
                token, action.CaseFile.Id).ConfigureAwait(false);
    }

    public async Task<IPaginatedResult<ActionViewDto>> GetReviewRequestsAsync(string userId, PaginatedRequest paging,
        CancellationToken token = default)
    {
        var count = await actionRepository.CountAsync(action =>
                !action.IsDeleted && action.CurrentReviewer != null && action.CurrentReviewer.Id.Equals(userId), token)
            .ConfigureAwait(false);

        var list = count > 0
            ? mapper.Map<IReadOnlyList<ActionViewDto>>(await actionRepository.GetPagedListAsync(action =>
                    !action.IsDeleted && action.CurrentReviewer != null && action.CurrentReviewer.Id.Equals(userId),
                paging, includeProperties:
                [
                    nameof(EnforcementAction.CaseFile), nameof(EnforcementAction.Reviews),
                    $"{nameof(EnforcementAction.CaseFile)}.{nameof(CaseFile.ResponsibleStaff)}"
                ],
                token).ConfigureAwait(false))
            : [];

        return new PaginatedResult<ActionViewDto>(list, count, paging);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        actionRepository.Dispose();
        caseFileRepository.Dispose();
        caseFileManager.Dispose();
        userService.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await actionRepository.DisposeAsync().ConfigureAwait(false);
        await caseFileRepository.DisposeAsync().ConfigureAwait(false);
        await caseFileManager.DisposeAsync().ConfigureAwait(false);
        userService.Dispose();
    }

    #endregion
}
