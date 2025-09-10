using AirWeb.Domain.Comments;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.EfRepository.Contexts;
using IaipDataService.Facilities;

namespace AirWeb.EfRepository.Repositories;

public sealed class EnforcementActionRepository(AppDbContext context)
    : BaseRepository<EnforcementAction, Guid, AppDbContext>(context), IEnforcementActionRepository
{
    public Task<ConsentOrder> GetConsentOrder(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<EnforcementActionType?> GetEnforcementActionType(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> OrderIdExists(short orderId, Guid? ignoreActionId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
