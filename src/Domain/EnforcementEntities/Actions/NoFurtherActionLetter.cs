using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class NoFurtherActionLetter : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private NoFurtherActionLetter() { }

    internal NoFurtherActionLetter(Guid id, NoticeOfViolation noticeOfViolation, ApplicationUser? user)
        : base(id, noticeOfViolation.CaseFile, user)
    {
        EnforcementActionType = EnforcementActionType.NoFurtherAction;
        NoticeOfViolation = noticeOfViolation;
    }

    public NoticeOfViolation NoticeOfViolation { get; set; } = null!;
}
