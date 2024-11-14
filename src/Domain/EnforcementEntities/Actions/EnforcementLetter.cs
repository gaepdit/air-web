using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class EnforcementLetter : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private EnforcementLetter() { }

    internal EnforcementLetter(Guid id, EnforcementCase enforcementCase, ApplicationUser? user)
        : base(id, enforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.Letter;
    }

    public bool ResponseRequested { get; set; }
    public DateOnly? ResponseReceived { get; set; }
}
