using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record CoViewDto : ActionViewDto, IIsResolved, IIsExecuted
{
    [Display(Name = "Signed copy received from facility")]
    public DateOnly? ReceivedFromFacility { get; init; }

    [Display(Name = "Executed")]
    public DateOnly? ExecutedDate { get; init; }

    public bool IsExecuted => ExecutedDate.HasValue;

    [Display(Name = "Received from Director's Office")]
    public DateOnly? ReceivedFromDirectorsOffice { get; init; }

    [Display(Name = "Resolved")]
    public DateOnly? ResolvedDate { get; init; }

    public bool IsResolved => ResolvedDate.HasValue;

    [Display(Name = "Order number")]
    public string? OrderNumber => OrderId == 0 ? null : $"{ConsentOrder.OrderNumberPrefix}{OrderId}";

    public short OrderId { get; init; }

    [Display(Name = "Penalty assessed")]
    public decimal? PenaltyAmount { get; init; }

    [Display(Name = "Penalty comment")]
    public string? PenaltyComment { get; init; }

    [Display(Name = "Defines stipulated penalties")]
    public bool StipulatedPenaltiesDefined { get; init; }

    [Display(Name = "Stipulated penalties received")]
    public ICollection<StipulatedPenaltyViewDto> StipulatedPenalties { get; } = [];
}
