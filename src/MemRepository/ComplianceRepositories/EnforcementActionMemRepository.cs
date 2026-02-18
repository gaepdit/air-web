using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using AirWeb.TestData.Enforcement;

namespace AirWeb.MemRepository.ComplianceRepositories;

public class EnforcementActionMemRepository()
    : BaseRepositoryWithMapping<EnforcementAction, Guid>(EnforcementActionData.GetData), IEnforcementActionRepository
{
    public Task<bool> OrderIdExists(short orderId, Guid? ignoreActionId, CancellationToken token = default) =>
        Task.FromResult(Items.OfType<ConsentOrder>()
            .Any(action =>
                action.Id != ignoreActionId &&
                !action.IsDeleted &&
                action.OrderId.Equals(orderId)));
}
