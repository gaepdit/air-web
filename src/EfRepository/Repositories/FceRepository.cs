using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.ValueObjects;
using AirWeb.EfRepository.DbContext;

namespace AirWeb.EfRepository.Repositories;

public sealed class FceRepository(AppDbContext context)
    : BaseRepository<Fce, int, AppDbContext>(context), IFceRepository
{
    // Entity Framework will set the ID automatically.
    public int? GetNextId() => null;

    public Task<bool> ExistsAsync(FacilityId facilityId, int year, CancellationToken token = default) =>
        Context.Fces.AsNoTracking().AnyAsync(fce => fce.FacilityId == facilityId && fce.Year.Equals(year), token);

    public async Task AddCommentAsync(int id, Comment comment, CancellationToken token = default)
    {
        var fce = await Context.Set<Fce>().AsNoTracking()
            .SingleAsync(fce => fce.Id.Equals(id), token).ConfigureAwait(false);
        fce.Comments.Add(comment);
        await UpdateAsync(fce, true, token).ConfigureAwait(false);
    }
}
