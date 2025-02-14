using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class ConsentOrder : EnforcementAction, IFormalEnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private ConsentOrder() { }

    internal ConsentOrder(Guid id, ProposedConsentOrder proposedConsentOrder, ApplicationUser? user)
        : base(id, proposedConsentOrder.CaseFile, user)
    {
        ActionType = EnforcementActionType.ConsentOrder;
        foreach (var action in proposedConsentOrder.ActionsToBeAddressed)
        {
            ActionsAddressed.Add(action);
        }

        ActionsAddressed.Add(proposedConsentOrder);
    }

    internal ConsentOrder(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.ConsentOrder;
    }

    public ICollection<IInformalEnforcementAction> ActionsAddressed { get; } = [];
    public OrderResolvedLetter? ResolvedLetter { get; set; }

    public DateOnly? ReceivedFromFacility { get; set; }
    public DateOnly? ExecutedDate { get; set; }
    public bool IsExecuted => ExecutedDate.HasValue;
    public DateOnly? ReceivedFromDirectorsOffice { get; set; }
    public DateOnly? ResolvedDate { get; set; }
    public bool IsResolved => ResolvedDate.HasValue;
    public short? OrderId { get; set; }

    [StringLength(13)]
    public string? OrderNumber
    {
        get => OrderId is null ? null : string.Concat("EPD-AQC-", OrderId.ToString());

        [UsedImplicitly]
        [SuppressMessage("ReSharper", "ValueParameterNotUsed")]
        [SuppressMessage("Blocker Code Smell", "S3237:\"value\" contextual keyword should be used")]
        private set
        {
            // Method intentionally left empty.
            // This allows storing read-only properties in the database.
            // See: https://github.com/dotnet/efcore/issues/13316#issuecomment-421052406
        }
    }

    [Precision(12, 2)]
    public decimal? PenaltyAmount { get; set; }

    [StringLength(7000)]
    public string? PenaltyComment { get; set; }

    public bool StipulatedPenaltiesDefined { get; set; }
    public ICollection<StipulatedPenalty> StipulatedPenalties { get; } = [];
}
