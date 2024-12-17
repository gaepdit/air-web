using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class NovNfaLetter : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private NovNfaLetter() { }

    internal NovNfaLetter(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        EnforcementActionType = EnforcementActionType.NovNfaLetter;
    }

    public bool ResponseRequested { get; set; }
    public DateOnly? ResponseReceived { get; set; }
}
