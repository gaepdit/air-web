using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class ConsentOrder : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private ConsentOrder() { }

    internal ConsentOrder(Guid id, ProposedConsentOrder proposedConsentOrder, ApplicationUser? user) :
        base(id, proposedConsentOrder.EnforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.ConsentOrder;
        ProposedConsentOrder = proposedConsentOrder;
    }

    internal ConsentOrder(Guid id, EnforcementCase enforcementCase, ApplicationUser? user) :
        base(id, enforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.ConsentOrder;
    }

    public ProposedConsentOrder? ProposedConsentOrder { get; set; }

    public DateOnly? ReceivedFromFacility { get; set; }
    public DateOnly? Executed { get; set; }
    public DateOnly? ReceivedFromDirectorsOffice { get; set; }
    public DateOnly? Resolved { get; set; }

    public short? OrderNumber { get; set; }
    public decimal? PenaltyAmount { get; set; }

    [StringLength(7000)]
    public string? PenaltyComment { get; set; }

    public bool StipulatedPenaltiesDefined { get; set; }
    public ICollection<StipulatedPenalty> StipulatedPenalties { get; } = [];
}
