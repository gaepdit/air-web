using System;
using AirWeb.Domain.Comments;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.EfRepository.Contexts;
using IaipDataService.Facilities;
using static System.Collections.Specialized.BitVector32;

namespace AirWeb.EfRepository.Repositories;

public sealed class EnforcementActionRepository(AppDbContext context)
    : BaseRepository<EnforcementAction, Guid, AppDbContext>(context), IEnforcementActionRepository
{
    public async Task<EnforcementActionType?> GetEnforcementActionType(Guid id, CancellationToken token = default) =>
        (await Context.Set<EnforcementAction>()
        .AsNoTracking()
        .Where(e => e.Id.Equals(id))
        .SingleOrDefaultAsync(token))
        ?.ActionType;
    

    public async Task<bool> OrderIdExists(short orderId, Guid? ignoreActionId, CancellationToken token = default) =>
        (await Context.Set<ConsentOrder>()
        .AsNoTracking()
        .AnyAsync(action =>
        action.Id != ignoreActionId
        && !action.IsDeleted
        && (action).OrderId.Equals(orderId)));


    public async Task<ConsentOrder> GetConsentOrder(Guid id, CancellationToken token = default) =>
        (await Context.Set<ConsentOrder>()
        .Include(e => e.StipulatedPenalties.Where(e => !e.IsDeleted))
        .SingleAsync(e => e.Id.Equals(id)));
}
