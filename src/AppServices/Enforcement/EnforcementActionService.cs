using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Users;
using AirWeb.Domain.EnforcementEntities;

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
        var enforcementAction =
            enforcementActionManager.CreateEnforcementAction(caseFile, resource.ActionType, resource.ResponseRequested,
                resource.Comment, currentUser);
        await enforcementActionRepository.InsertAsync(enforcementAction, token: token).ConfigureAwait(false);
        return enforcementAction.Id;
    }
}
