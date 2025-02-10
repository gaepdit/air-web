using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class ProposedConsentOrder : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private ProposedConsentOrder() { }

    internal ProposedConsentOrder(Guid id, NoticeOfViolation noticeOfViolation, ApplicationUser? user)
        : base(id, noticeOfViolation.CaseFile, user)
    {
        ActionType = EnforcementActionType.ProposedConsentOrder;
        NoticeOfViolation = noticeOfViolation;
    }

    internal ProposedConsentOrder(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.ProposedConsentOrder;
    }

    public NoticeOfViolation? NoticeOfViolation { get; set; }

    public DateOnly? ResponseReceived { get; set; }
}
