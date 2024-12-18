using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class ConsentOrder : EnforcementAction, IExecutable
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private ConsentOrder() { }

    internal ConsentOrder(Guid id, ProposedConsentOrder proposedConsentOrder, ApplicationUser? user) :
        base(id, proposedConsentOrder.CaseFile, user)
    {
        ActionType = EnforcementActionType.ConsentOrder;
        ProposedConsentOrder = proposedConsentOrder;
    }

    internal ConsentOrder(Guid id, CaseFile caseFile, ApplicationUser? user) :
        base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.ConsentOrder;
    }

    public ProposedConsentOrder? ProposedConsentOrder { get; set; }

    public DateOnly? ReceivedFromFacility { get; set; }
    public DateOnly? ExecutedDate { get; set; }
    public bool IsExecuted => ExecutedDate.HasValue;
    public DateOnly? ReceivedFromDirectorsOffice { get; set; }
    public DateOnly? ResolvedDate { get; set; }
    public bool IsResolved => ResolvedDate.HasValue;
    public CoResolvedLetter? ResolvedLetter { get; set; }

    public short? OrderNumber { get; set; }

    [Precision(12, 2)]
    public decimal? PenaltyAmount { get; set; }

    [StringLength(7000)]
    public string? PenaltyComment { get; set; }

    public bool StipulatedPenaltiesDefined { get; set; }
    public ICollection<StipulatedPenalty> StipulatedPenalties { get; } = [];
}
