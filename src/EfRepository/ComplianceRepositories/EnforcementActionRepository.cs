using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using AirWeb.EfRepository.Contexts;

namespace AirWeb.EfRepository.ComplianceRepositories;

public sealed class EnforcementActionRepository(AppDbContext context)
    : BaseRepositoryWithMapping<EnforcementAction, Guid, AppDbContext>(context), IEnforcementActionRepository
{
    public async Task<bool> OrderIdExists(short orderId, Guid? ignoreActionId, CancellationToken token = default) =>
        await Context.Set<ConsentOrder>()
            .AnyAsync(action =>
                action.Id != ignoreActionId &&
                !action.IsDeleted &&
                action.OrderId.Equals(orderId), token).ConfigureAwait(false);
}
