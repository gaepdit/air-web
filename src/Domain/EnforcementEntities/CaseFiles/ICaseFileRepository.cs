using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.EnforcementEntities.ViolationTypes;

namespace AirWeb.Domain.EnforcementEntities.CaseFiles;

public interface ICaseFileRepository : IRepositoryWithMapping<CaseFile, int>, ICommentRepository<int>
{
    public static string[] IncludeDetails =>
    [
        nameof(CaseFile.ComplianceEvents),
        nameof(CaseFile.EnforcementActions),
        nameof(CaseFile.ViolationType),
        $"{nameof(CaseFile.EnforcementActions)}.{nameof(ConsentOrder.StipulatedPenalties)}",
        $"{nameof(CaseFile.EnforcementActions)}.{nameof(EnforcementAction.Reviews)}",
        $"{nameof(CaseFile.EnforcementActions)}.{nameof(EnforcementAction.Reviews)}.{nameof(EnforcementActionReview.RequestedOf)}",
        $"{nameof(CaseFile.EnforcementActions)}.{nameof(EnforcementAction.Reviews)}.{nameof(EnforcementActionReview.RequestedBy)}",
    ];

    // Will return the next available ID if the repository requires it for adding new entities (e.g., local repository).
    // Will return null if the repository creates a new ID on insert (e.g., Entity Framework).
    int? GetNextId();

    // Case File details
    Task<ViolationType?> GetViolationTypeAsync(string? code, CancellationToken token = default);
    Task<IEnumerable<Pollutant>> GetPollutantsAsync(int id, CancellationToken token = default);
    Task<IEnumerable<AirProgram>> GetAirProgramsAsync(int id, CancellationToken token = default);
}
