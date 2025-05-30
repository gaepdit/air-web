using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.TestData.Enforcement;

namespace AirWeb.LocalRepository.Repositories;

public class LocalEnforcementActionRepository()
    : BaseRepository<EnforcementAction, Guid>(EnforcementActionData.GetData), IEnforcementActionRepository
{
    public Task<EnforcementActionType?> GetEnforcementActionType(Guid id, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(action => action.Id.Equals(id))?.ActionType);

    public Task<bool> OrderIdExists(short orderId, Guid? ignoreActionId, CancellationToken token = default) =>
        Task.FromResult(Items.Any(action =>
            action.ActionType == EnforcementActionType.ConsentOrder &&
            action.Id != ignoreActionId && !action.IsDeleted &&
            ((ConsentOrder)action).OrderId.Equals(orderId)));

    public async Task<ConsentOrder> GetConsentOrder(Guid id, CancellationToken token = default)
    {
        var consentOrder = (ConsentOrder)await GetAsync(id, token: token).ConfigureAwait(false);
        consentOrder.StipulatedPenalties.RemoveAll(p => p.IsDeleted);
        return consentOrder;
    }
}
