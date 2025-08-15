using AirWeb.Domain.Comments;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.EfRepository.Contexts;
using IaipDataService.Facilities;

namespace AirWeb.EfRepository.Repositories;

public sealed class CaseFileRepository(AppDbContext context)
    : BaseRepository<CaseFile, int, AppDbContext>(context), ICaseFileRepository
{
    public int? GetNextId()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Pollutant>> GetPollutantsAsync(int id, CancellationToken token = default) =>
        (await Context.Set<CaseFile>()
            .Where(entry => entry.Id.Equals(id))
            .SingleAsync(token).ConfigureAwait(false)).GetPollutants();

    public async Task<IEnumerable<AirProgram>> GetAirProgramsAsync(int id, CancellationToken token = default) =>
        (await Context.Set<CaseFile>()
            .Where(entry => entry.Id.Equals(id))
            .SingleAsync(token).ConfigureAwait(false)).AirPrograms;

    // For later
    public Task AddCommentAsync(int itemId, Comment comment, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCommentAsync(Guid commentId, string? userId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
