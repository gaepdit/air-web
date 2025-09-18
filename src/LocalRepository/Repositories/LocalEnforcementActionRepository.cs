using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.TestData.Enforcement;

namespace AirWeb.LocalRepository.Repositories;

public class LocalEnforcementActionRepository()
    : BaseRepository<EnforcementAction, Guid>(EnforcementActionData.GetData), IEnforcementActionRepository
{
    public Task<EnforcementActionType?> GetEnforcementActionType(Guid id, CancellationToken token = default) =>
        Task.FromResult(Items
            .SingleOrDefault(action => action.Id.Equals(id))
            ?.ActionType);

    public Task<bool> OrderIdExists(short orderId, Guid? ignoreActionId, CancellationToken token = default) =>
        Task.FromResult(Items.OfType<ConsentOrder>()
            .Any(action =>
                action.Id != ignoreActionId &&
                !action.IsDeleted &&
                action.OrderId.Equals(orderId)));

    public Task<ConsentOrder> GetConsentOrder(Guid id, CancellationToken token = default)
    {
        var consentOrder = Items.OfType<ConsentOrder>()
            .Single(e => e.Id.Equals(id));
        consentOrder.StipulatedPenalties.RemoveAll(p => p.IsDeleted);
        return Task.FromResult(consentOrder);
    }
}
