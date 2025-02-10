using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class NoticeOfViolation : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private NoticeOfViolation() { }

    internal NoticeOfViolation(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.NoticeOfViolation;
    }

    public bool ResponseRequested { get; set; } = true;
    public DateOnly? ResponseReceived { get; set; }

    public NoFurtherActionLetter? NoFurtherActionLetter { get; set; }
}
