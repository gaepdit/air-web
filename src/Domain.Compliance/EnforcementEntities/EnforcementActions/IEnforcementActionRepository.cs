namespace AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;

public interface IEnforcementActionRepository : IRepositoryWithMapping<EnforcementAction>
{
    Task<bool> OrderIdExists(short orderId, Guid? ignoreActionId, CancellationToken token = default);
}
