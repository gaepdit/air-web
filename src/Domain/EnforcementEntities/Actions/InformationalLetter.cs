using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class InformationalLetter : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private InformationalLetter() { }

    internal InformationalLetter(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.InformationalLetter;
    }

    public bool ResponseRequested { get; set; }
    public DateOnly? ResponseReceived { get; set; }
}
