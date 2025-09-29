using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.TestData.Enforcement;

namespace AirWeb.LocalRepository.Repositories;

public class LocalEnforcementActionRepository()
    : BaseRepositoryWithMapping<EnforcementAction, Guid>(EnforcementActionData.GetData), IEnforcementActionRepository
{
    public Task<bool> OrderIdExists(short orderId, Guid? ignoreActionId, CancellationToken token = default) =>
        Task.FromResult(Items.OfType<ConsentOrder>()
            .Any(action =>
                action.Id != ignoreActionId &&
                !action.IsDeleted &&
                action.OrderId.Equals(orderId)));

    public Task<ConsentOrder?> FindConsentOrder(Guid id, CancellationToken token = default)
    {
        var consentOrder = Items.OfType<ConsentOrder>()
            .SingleOrDefault(e => e.Id.Equals(id));
        if (consentOrder == null) return Task.FromResult<ConsentOrder?>(null);
        consentOrder.StipulatedPenalties.RemoveAll(p => p.IsDeleted);
        return Task.FromResult<ConsentOrder?>(consentOrder);
    }
}
