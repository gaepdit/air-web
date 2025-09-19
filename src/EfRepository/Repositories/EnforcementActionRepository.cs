using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.EfRepository.Contexts;

namespace AirWeb.EfRepository.Repositories;

public sealed class EnforcementActionRepository(AppDbContext context)
    : BaseRepository<EnforcementAction, Guid, AppDbContext>(context), IEnforcementActionRepository
{
    public async Task<EnforcementActionType?> GetEnforcementActionType(Guid id, CancellationToken token = default) =>
        (await Context.Set<EnforcementAction>()
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token).ConfigureAwait(false))
        ?.ActionType;


    public async Task<bool> OrderIdExists(short orderId, Guid? ignoreActionId, CancellationToken token = default) =>
        await Context.Set<ConsentOrder>()
            .AsNoTracking()
            .AnyAsync(action =>
                action.Id != ignoreActionId &&
                !action.IsDeleted &&
                action.OrderId.Equals(orderId), token).ConfigureAwait(false);


    public async Task<ConsentOrder?> FindConsentOrder(Guid id, CancellationToken token = default) =>
        await Context.Set<ConsentOrder>()
            .Include(e => e.StipulatedPenalties
                .Where(p => !p.IsDeleted))
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token).ConfigureAwait(false);
}
