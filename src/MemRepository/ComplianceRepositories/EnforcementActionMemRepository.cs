using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using AirWeb.TestData.Enforcement;
using System.Linq.Expressions;

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

    public Task<DxActionEnforcementAction?> FindDxActionEnforcementAsync(
        Expression<Func<DxActionEnforcementAction, bool>> predicate, CancellationToken token) =>
        Task.FromResult(Items.OfType<DxActionEnforcementAction>().SingleOrDefault(predicate.Compile()));
}
