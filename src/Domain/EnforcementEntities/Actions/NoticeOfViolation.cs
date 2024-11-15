using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class NoticeOfViolation : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private NoticeOfViolation() { }

    internal NoticeOfViolation(Guid id, EnforcementCase enforcementCase, ApplicationUser? user)
        : base(id, enforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.NoticeOfViolation;
    }

    public bool ResponseRequested { get; set; } = true;
    public DateOnly? ResponseReceived { get; set; }

    public NoFurtherActionLetter? NoFurtherActionLetter { get; set; }
}
