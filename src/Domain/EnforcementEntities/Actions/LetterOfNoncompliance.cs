using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class LetterOfNoncompliance : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private LetterOfNoncompliance() { }

    internal LetterOfNoncompliance(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.LetterOfNoncompliance;
    }

    public bool ResponseRequested { get; set; }
    public DateOnly? ResponseReceived { get; set; }
}
