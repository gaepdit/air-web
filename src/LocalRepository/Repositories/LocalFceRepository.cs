using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.TestData.Compliance;
using IaipDataService.Facilities;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalFceRepository()
    : BaseRepositoryWithMapping<Fce, int>(FceData.GetData), IFceRepository
{
    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Count == 0 ? 1 : Items.Select(fce => fce.Id).Max() + 1;

    public async Task<Fce?> FindWithDetailsAsync(int id, CancellationToken token = default)
    {
        var fce = await FindAsync(id, token: token).ConfigureAwait(false);
        fce?.Comments.RemoveAll(comment => comment.IsDeleted);
        return fce;
    }

    public Task<bool> ExistsAsync(FacilityId facilityId, int year, int? ignoreId = null,
        CancellationToken token = default) => Task.FromResult(Items.Any(fce =>
        fce.FacilityId == facilityId && fce.Year.Equals(year) && !fce.IsDeleted && fce.Id != ignoreId));
}
