using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.TestData.Enforcement;

namespace AirWeb.LocalRepository.Repositories;

public class LocalEnforcementActionRepository()
    : BaseRepository<EnforcementAction, Guid>(EnforcementActionData.GetData), IEnforcementActionRepository
{
    public Task<EnforcementActionType?> GetEnforcementActionType(Guid id, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(action => action.Id.Equals(id))?.ActionType);
}
