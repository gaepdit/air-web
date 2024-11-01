using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class LetterOfNoncompliance : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private LetterOfNoncompliance() { }

    internal LetterOfNoncompliance(Guid id, EnforcementCase enforcementCase, ApplicationUser? user)
        : base(id, enforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.LetterOfNoncompliance;
    }

    // Properties
    public bool ResponseRequested { get; set; }
    public DateOnly? ResponseReceived { get; set; }
}
