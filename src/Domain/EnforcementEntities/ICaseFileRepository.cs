using AirWeb.Domain.EnforcementEntities.Cases;

namespace AirWeb.Domain.EnforcementEntities;

public interface ICaseFileRepository : IRepository<CaseFile, int>, ICommentRepository<int>
{
    public static string[] IncludeDetails => [nameof(CaseFile.EnforcementActions), nameof(CaseFile.ComplianceEvents)];

    // Will return the next available ID if the repository requires it for adding new entities (e.g., local repository).
    // Will return null if the repository creates a new ID on insert (e.g., Entity Framework).
    int? GetNextId();
}
