namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public interface IEnforcementActionRepository : IRepositoryWithMapping<EnforcementAction>
{
    Task<bool> OrderIdExists(short orderId, Guid? ignoreActionId, CancellationToken token = default);
    Task<ConsentOrder?> FindConsentOrder(Guid id, CancellationToken token = default);
}
