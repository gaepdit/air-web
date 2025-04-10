namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public interface IEnforcementActionRepository : IRepository<EnforcementAction>
{
    /// <summary>
    /// Gets the <see cref="EnforcementActionType"/> for the specified <see cref="EnforcementAction"/>.
    /// </summary>
    /// <param name="id">The ID of the enforcement action.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The <see cref="EnforcementActionType"/> of the enforcement action.</returns>
    Task<EnforcementActionType?> GetEnforcementActionType(Guid id, CancellationToken token = default);

    Task<bool> OrderIdExists(short orderId, Guid? ignoreActionId, CancellationToken token = default);
}
