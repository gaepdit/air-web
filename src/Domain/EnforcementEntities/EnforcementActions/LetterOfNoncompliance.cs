using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class LetterOfNoncompliance : EnforcementAction, IResponseRequested, IResolvable
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private LetterOfNoncompliance() { }

    internal LetterOfNoncompliance(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.LetterOfNoncompliance;
    }

    public bool ResponseRequested { get; set; } = true;
    public DateOnly? ResponseReceived { get; set; }

    [StringLength(7000)]
    public string? ResponseComment { get; set; }

    public DateOnly? ResolvedDate { get; internal set; }
    public bool IsResolved => ResolvedDate.HasValue;
    public void Resolve(DateOnly resolvedDate) => ResolvedDate = resolvedDate;
}
