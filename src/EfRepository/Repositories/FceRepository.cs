using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.Comments;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.EfRepository.Contexts;
using IaipDataService.Facilities;

namespace AirWeb.EfRepository.Repositories;

public sealed class FceRepository(AppDbContext context)
    : BaseRepository<Fce, int, AppDbContext>(context), IFceRepository
{
    // Entity Framework will set the ID automatically.
    public int? GetNextId() => null;

    public Task<Fce?> FindWithExtrasAsync(int id, CancellationToken token = default) =>
        Context.Set<Fce>().AsNoTracking()
            .Include(fce => fce.Comments
                .Where(comment => !comment.DeletedAt.HasValue)
                .OrderBy(comment => comment.CommentedAt).ThenBy(comment => comment.Id))
            .Include(fce => fce.AuditPoints
                .OrderBy(audit => audit.When).ThenBy(audit => audit.Id))
            .SingleOrDefaultAsync(fce => fce.Id.Equals(id), token);

    public Task<bool> ExistsAsync(FacilityId facilityId, int year, int? ignoreId = null,
        CancellationToken token = default) =>
        Context.Fces.AsNoTracking().AnyAsync(fce =>
            fce.FacilityId.Equals(facilityId) && fce.Year.Equals(year) && !fce.IsDeleted && fce.Id != ignoreId, token);

    public async Task<List<FceAuditPoint>> GetAuditPointsAsync(int id, CancellationToken token = default) =>
        (await Context.Set<Fce>().AsNoTracking()
            .Include(fce => fce.AuditPoints
                .OrderBy(audit => audit.When)
                .ThenBy(audit => audit.Id)
            ).SingleAsync(fce => fce.Id.Equals(id), token).ConfigureAwait(false))
        .AuditPoints;

    public async Task AddCommentAsync(int itemId, Comment comment, CancellationToken token = default)
    {
        Context.FceComments.Add(new FceComment(comment, itemId));
        await SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task DeleteCommentAsync(Guid commentId, string? userId, CancellationToken token = default)
    {
        var comment = await Context.FceComments.FindAsync([commentId], token).ConfigureAwait(false);
        if (comment != null)
        {
            comment.SetDeleted(userId);
            await SaveChangesAsync(token).ConfigureAwait(false);
        }
    }
}
