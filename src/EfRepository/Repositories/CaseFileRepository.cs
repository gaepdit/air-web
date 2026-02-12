using AirWeb.Domain.Comments;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.ViolationTypes;
using AirWeb.EfRepository.Contexts;
using IaipDataService.Facilities;

namespace AirWeb.EfRepository.Repositories;

public sealed class CaseFileRepository(AppDbContext context)
    : BaseRepositoryWithMapping<CaseFile, int, AppDbContext>(context), ICaseFileRepository
{
    // Entity Framework will set the ID automatically.
    public int? GetNextId() => null;

    public Task<CaseFile?> FindWithDetailsAsync(int id, CancellationToken token = default) =>
        Context.Set<CaseFile>().AsNoTracking()
            .Include(caseFile => caseFile.Comments
                .Where(comment => !comment.DeletedAt.HasValue)
                .OrderBy(comment => comment.CommentedAt).ThenBy(comment => comment.Id))
            .Include(caseFile => caseFile.AuditPoints
                .OrderBy(audit => audit.When).ThenBy(audit => audit.Id))
            .Include(caseFile => caseFile.ComplianceEvents)
            .Include(caseFile => caseFile.ViolationType)
            .Include(caseFile => caseFile.EnforcementActions)
            .ThenInclude(action => action.Reviews).ThenInclude(review => review.RequestedBy)
            .Include(caseFile => caseFile.EnforcementActions)
            .ThenInclude(action => action.Reviews).ThenInclude(review => review.RequestedOf)
            .Include(caseFile => caseFile.EnforcementActions)
            .ThenInclude(action => ((ConsentOrder)action).StipulatedPenalties)
            .SingleOrDefaultAsync(fce => fce.Id.Equals(id), token);

    public async Task<ViolationType?> GetViolationTypeAsync(string? code, CancellationToken token = default) =>
        await Context.ViolationTypes.AsTracking().SingleOrDefaultAsync(v => v.Code == code, token)
            .ConfigureAwait(false);

    public async Task<IEnumerable<Pollutant>> GetPollutantsAsync(int id, CancellationToken token = default) =>
        (await GetAsync(id, token).ConfigureAwait(false)).GetPollutants();

    public async Task<IEnumerable<AirProgram>> GetAirProgramsAsync(int id, CancellationToken token = default) =>
        (await GetAsync(id, token).ConfigureAwait(false)).AirPrograms;

    public async Task AddCommentAsync(int itemId, Comment comment, CancellationToken token = default)
    {
        await Context.CaseFileComments.AddAsync(new CaseFileComment(comment, itemId), token).ConfigureAwait(false);
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
