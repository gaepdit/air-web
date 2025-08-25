using AirWeb.Domain.Comments;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.EfRepository.Contexts;
using IaipDataService.Facilities;

namespace AirWeb.EfRepository.Repositories;

public sealed class CaseFileRepository(AppDbContext context)
    : BaseRepositoryWithMapping<CaseFile, int, AppDbContext>(context), ICaseFileRepository
{
    // Entity Framework will set the ID automatically.
    public int? GetNextId() => null;

    public async Task<IEnumerable<Pollutant>> GetPollutantsAsync(int id, CancellationToken token = default) =>
        (await Context.Set<CaseFile>()
            .Where(entry => entry.Id.Equals(id))
            .SingleAsync(token).ConfigureAwait(false)).GetPollutants();

    public async Task<IEnumerable<AirProgram>> GetAirProgramsAsync(int id, CancellationToken token = default) =>
        (await Context.Set<CaseFile>()
            .Where(entry => entry.Id.Equals(id))
            .SingleAsync(token).ConfigureAwait(false)).AirPrograms;

    public async Task AddCommentAsync(int itemId, Comment comment, CancellationToken token = default)
    {
        Context.CaseFileComments.Add(new CaseFileComment(comment, itemId));
        await SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task DeleteCommentAsync(Guid commentId, string? userId, CancellationToken token = default)
    {
        var comment = await Context.CaseFileComments.FindAsync([commentId], token).ConfigureAwait(false);
        if (comment != null)
        {
            comment.SetDeleted(userId);
            await SaveChangesAsync(token).ConfigureAwait(false);
        }
    }
}
