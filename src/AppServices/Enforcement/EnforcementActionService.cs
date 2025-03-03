using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Users;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Enforcement;

public class EnforcementActionService(
    IEnforcementActionManager enforcementActionManager,
    IEnforcementActionRepository enforcementActionRepository,
    ICaseFileRepository caseFileRepository,
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

    public async Task AddResponse(Guid id, CommentAndMaxDateDto responseDto, CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await enforcementActionRepository.GetAsync(id, token).ConfigureAwait(false);
        enforcementActionManager.AddResponse(enforcementAction, responseDto.Date, responseDto.Comment, currentUser);
        await enforcementActionRepository.UpdateAsync(enforcementAction, token: token).ConfigureAwait(false);
    }

    public async Task IssueAsync(Guid id, MaxCurrentDateOnlyDto dateDto, CancellationToken token = default)
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

    public async Task DeleteAsync(Guid id, CancellationToken token)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var enforcementAction = await enforcementActionRepository.GetAsync(id, token).ConfigureAwait(false);
        enforcementActionManager.Delete(enforcementAction, currentUser);
        await enforcementActionRepository.UpdateAsync(enforcementAction, token: token).ConfigureAwait(false);
    }
}
