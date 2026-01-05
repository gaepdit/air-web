using AirWeb.Domain.DataAttributes;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class ConsentOrder : ReportableEnforcementAction, IFormalEnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private ConsentOrder() { }

    internal ConsentOrder(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.ConsentOrder;
    }

    public DateOnly? ReceivedFromFacility { get; set; }
    public DateOnly? ExecutedDate { get; set; }
    public bool IsExecuted => ExecutedDate.HasValue;
    public void Execute(DateOnly executedDate) => ExecutedDate = executedDate;

    public DateOnly? ReceivedFromDirectorsOffice { get; set; }
    public DateOnly? ResolvedDate { get; set; }
    public bool IsResolved => ResolvedDate.HasValue;
    public void Resolve(DateOnly resolvedDate) => ResolvedDate = resolvedDate;

    // Required for new data but nullable for historical data.
    [PositiveShort]
    public short? OrderId { get; set; }

    public const string OrderNumberPrefix = "EPD-AQC-";
    public string? OrderNumber => OrderId == null ? null : $"{OrderNumberPrefix}{OrderId}";

    [Precision(precision: 18, scale: 2)]
    [PositiveDecimal]
    public decimal? PenaltyAmount { get; init; }

    [StringLength(7000)]
    public string? PenaltyComment { get; init; }

    public bool StipulatedPenaltiesDefined { get; init; }
    public List<StipulatedPenalty> StipulatedPenalties { get; } = [];
}
