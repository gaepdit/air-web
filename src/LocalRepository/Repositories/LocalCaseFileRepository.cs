using AirWeb.Domain.Comments;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.TestData.Enforcement;
using IaipDataService.Facilities;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalCaseFileRepository : BaseRepositoryWithMapping<CaseFile, int>, ICaseFileRepository
{
    public LocalCaseFileRepository() : base(CaseFileData.GetData) => _ = EnforcementActionData.GetData;

    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Count == 0 ? 1 : Items.Select(caseFile => caseFile.Id).Max() + 1;

    // Pollutants & Air Programs
    public Task<IEnumerable<Pollutant>> GetPollutantsAsync(int id, CancellationToken token = default) =>
        Task.FromResult<IEnumerable<Pollutant>>(Items.Single(caseFile => caseFile.Id.Equals(id)).GetPollutants());

    public Task<IEnumerable<AirProgram>> GetAirProgramsAsync(int id, CancellationToken token = default) =>
        Task.FromResult<IEnumerable<AirProgram>>(Items.Single(caseFile => caseFile.Id.Equals(id)).AirPrograms);

    public async Task AddCommentAsync(int itemId, Comment comment, CancellationToken token = default) =>
        (await GetAsync(itemId, token: token).ConfigureAwait(false)).Comments.Add(new CaseFileComment(comment, itemId));

    public Task DeleteCommentAsync(Guid commentId, string? userId, CancellationToken token = default)
    {
        var comment = Items.SelectMany(caseFile => caseFile.Comments)
            .FirstOrDefault(comment => comment.Id == commentId);

        if (comment != null)
        {
            var caseFile = Items.First(caseFile => caseFile.Comments.Contains(comment));
            caseFile.Comments.Remove(comment);
        }

        return Task.CompletedTask;
    }
}
