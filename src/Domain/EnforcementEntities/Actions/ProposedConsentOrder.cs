using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class ProposedConsentOrder : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private ProposedConsentOrder() { }

    internal ProposedConsentOrder(Guid id, NoticeOfViolation noticeOfViolation, ApplicationUser? user) :
        base(id, noticeOfViolation.EnforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.ProposedConsentOrder;
        NoticeOfViolation = noticeOfViolation;
    }

    internal ProposedConsentOrder(Guid id, EnforcementCase enforcementCase, ApplicationUser? user) :
        base(id, enforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.ProposedConsentOrder;
    }

    public NoticeOfViolation NoticeOfViolation { get; set; } = null!;

    public DateOnly? ResponseReceived { get; set; }
}
