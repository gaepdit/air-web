using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record CoViewDto : ActionViewDto, IIsResolved, IIsExecuted
{
    [Display(Name = "Signed Copy Received From Facility")]
    public DateOnly? ReceivedFromFacility { get; init; }

    [Display(Name = "Executed")]
    public DateOnly? ExecutedDate { get; init; }

    public bool IsExecuted => ExecutedDate.HasValue;

    [Display(Name = "Received From Director's Office")]
    public DateOnly? ReceivedFromDirectorsOffice { get; init; }

    [Display(Name = "Resolved")]
    public DateOnly? ResolvedDate { get; init; }

    public bool IsResolved => ResolvedDate.HasValue;

    [Display(Name = "Order Number")]
    public string? OrderNumber => OrderId == 0 ? null : $"{ConsentOrder.OrderNumberPrefix}{OrderId}";

    public short OrderId { get; init; }

    [Display(Name = "Penalty Assessed")]
    public decimal? PenaltyAmount { get; init; }

    [Display(Name = "Penalty Comment")]
    public string? PenaltyComment { get; init; }

    [Display(Name = "Defines Stipulated Penalties")]
    public bool StipulatedPenaltiesDefined { get; init; }

    [Display(Name = "Stipulated Penalties Received")]
    public ICollection<StipulatedPenaltyViewDto> StipulatedPenalties { get; } = [];
}
