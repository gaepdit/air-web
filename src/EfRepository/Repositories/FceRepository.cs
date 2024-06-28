using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.EfRepository.DbContext;

namespace AirWeb.EfRepository.Repositories;

public sealed class FceRepository(AppDbContext context)
    : BaseRepository<Fce, int, AppDbContext>(context), IFceRepository
{
    // Entity Framework will set the ID automatically.
    public int? GetNextId() => null;

    public Task<bool> ExistsAsync(FacilityId facilityId, int year, CancellationToken token = default) =>
        Context.Fces.AsNoTracking().AnyAsync(fce => fce.Facility.Id == facilityId && fce.Year.Equals(year), token);
}
