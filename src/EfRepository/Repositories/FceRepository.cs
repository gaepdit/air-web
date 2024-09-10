using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.ValueObjects;
using AirWeb.EfRepository.DbContext;
using GaEpd.AppLibrary.Domain.Repositories;

namespace AirWeb.EfRepository.Repositories;

public sealed class FceRepository(AppDbContext context)
    : BaseRepository<Fce, int, AppDbContext>(context), IFceRepository
{
    // Entity Framework will set the ID automatically.
    public int? GetNextId() => null;

    public Task<Fce?> FindWithCommentsAsync(int id, CancellationToken token = default) =>
        Context.Set<Fce>().AsNoTracking()
            .Include(fce => fce.Comments)
            .SingleOrDefaultAsync(fce => fce.Id.Equals(id), token);

    public Task<bool> ExistsAsync(FacilityId facilityId, int year, int? ignoreId = null,
        CancellationToken token = default) =>
        Context.Fces.AsNoTracking().AnyAsync(fce =>
            fce.FacilityId.Equals(facilityId) && fce.Year.Equals(year) && !fce.IsDeleted && fce.Id != ignoreId, token);

    public async Task AddCommentAsync(int id, Comment comment, CancellationToken token = default)
    {
        Context.FceComments.Add(new FceComment(comment, id));
        await SaveChangesAsync(token).ConfigureAwait(false);
    }
}
