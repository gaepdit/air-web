using System.Linq.Expressions;

namespace AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;

public interface IEnforcementActionRepository : IRepositoryWithMapping<EnforcementAction>
{
    Task<bool> OrderIdExists(short orderId, Guid? ignoreActionId, CancellationToken token = default);

    Task<DxActionEnforcementAction?> FindDxActionEnforcementAsync(
        Expression<Func<DxActionEnforcementAction, bool>> predicate, CancellationToken token);
}
