namespace AirWeb.Domain.EnforcementEntities.CaseFiles;

public interface ICaseFileRepository : IRepositoryWithMapping<CaseFile, int>, ICommentRepository<int>
{
    public static string[] IncludeDetails => [nameof(CaseFile.EnforcementActions), nameof(CaseFile.ComplianceEvents)];

    // Will return the next available ID if the repository requires it for adding new entities (e.g., local repository).
    // Will return null if the repository creates a new ID on insert (e.g., Entity Framework).
    int? GetNextId();

    // Pollutants & Air Programs
    Task<IEnumerable<Pollutant>> GetPollutantsAsync(int id, CancellationToken token = default);
    Task<IEnumerable<AirProgram>> GetAirProgramsAsync(int id, CancellationToken token = default);
}
