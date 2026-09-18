using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using AirWeb.EfRepository.Contexts;
using System.Linq.Expressions;

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

    public Task<DxActionEnforcementAction?> FindDxActionEnforcementAsync(
        Expression<Func<DxActionEnforcementAction, bool>> predicate, CancellationToken token) =>
        Context.Set<DxActionEnforcementAction>().AsNoTracking().Where(predicate)
            .Include(dx => dx.CaseFile).SingleOrDefaultAsync(token);
}
