using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.EnforcementEntities.ViolationTypes;

namespace AirWeb.Domain.EnforcementEntities.CaseFiles;

public interface ICaseFileRepository : IRepositoryWithMapping<CaseFile, int>, ICommentRepository<int>
{
    // Will return the next available ID if the repository requires it for adding new entities (e.g., local repository).
    // Will return null if the repository creates a new ID on insert (e.g., Entity Framework).
    int? GetNextId();

    /// <summary>
    /// Returns the <see cref="CaseFile"/> with the given <paramref name="id"/>. Returns null if there are no matches.
    /// The returned entity will include the Comments and Audit Points navigation properties.
    /// </summary>
    /// <param name="id">The ID of the Case File</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A Case File with included details or null.</returns>
    Task<CaseFile?> FindWithDetailsAsync(int id, CancellationToken token = default);
    
    // Case File details
    Task<ViolationType?> GetViolationTypeAsync(string? code, CancellationToken token = default);
    Task<IEnumerable<Pollutant>> GetPollutantsAsync(int id, CancellationToken token = default);
    Task<IEnumerable<AirProgram>> GetAirProgramsAsync(int id, CancellationToken token = default);
}
