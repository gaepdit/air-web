using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class NovNfaLetter : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private NovNfaLetter() { }

    internal NovNfaLetter(Guid id, EnforcementCase enforcementCase, ApplicationUser? user)
        : base(id, enforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.NovNfa;
    }

    public bool ResponseRequested { get; set; }
    public DateOnly? ResponseReceived { get; set; }
}
