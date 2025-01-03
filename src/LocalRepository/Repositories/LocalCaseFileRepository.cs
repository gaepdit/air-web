using AirWeb.Domain.Comments;
using AirWeb.Domain.EnforcementEntities;
using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.TestData.Enforcement;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalCaseFileRepository(IEnforcementActionRepository actionRepository)
    : BaseRepository<CaseFile, int>(CaseFileData.GetData), ICaseFileRepository
{
    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Count == 0 ? 1 : Items.Select(caseFile => caseFile.Id).Max() + 1;

    public async Task<CaseFile?> FindDetailedCaseFileAsync(int id, CancellationToken token = default)
    {
        var caseFile = await FindAsync(id, token).ConfigureAwait(false);
        if (caseFile == null) return null;

        caseFile.EnforcementActions.Clear();
        caseFile.EnforcementActions.AddRange(await actionRepository
            .GetListAsync(action => action.CaseFile.Id.Equals(id), token).ConfigureAwait(false));

        return caseFile;
    }

    public async Task AddCommentAsync(int itemId, Comment comment, CancellationToken token = default) =>
        (await GetAsync(itemId, token).ConfigureAwait(false)).Comments.Add(new CaseFileComment(comment, itemId));

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
