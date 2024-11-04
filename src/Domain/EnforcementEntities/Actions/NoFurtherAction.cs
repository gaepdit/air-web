using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class NoFurtherAction : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private NoFurtherAction() { }

    internal NoFurtherAction(Guid id, NoticeOfViolation noticeOfViolation, ApplicationUser? user)
        : base(id, noticeOfViolation.EnforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.NoFurtherAction;
        NoticeOfViolation = noticeOfViolation;
    }

    public NoticeOfViolation NoticeOfViolation { get; set; } = null!;
}
