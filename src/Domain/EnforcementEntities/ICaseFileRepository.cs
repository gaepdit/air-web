using AirWeb.Domain.EnforcementEntities.Cases;

namespace AirWeb.Domain.EnforcementEntities;

public interface ICaseFileRepository : IRepository<CaseFile, int>, ICommentRepository<int>
{
    public static string[] IncludeComments => [nameof(CaseFile.Comments)];

    // Will return the next available ID if the repository requires it for adding new entities (e.g., local repository).
    // Will return null if the repository creates a new ID on insert (e.g., Entity Framework).
    int? GetNextId();

    /// <summary>
    /// Returns the <see cref="CaseFile"/> with the given <paramref name="id"/>. Returns null if there are no matches.
    /// The returned entity will include the Comments navigation property.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A Case File with Comments included or null.</returns>
    Task<CaseFile?> FindWithCommentsAsync(int id, CancellationToken token = default);
}
