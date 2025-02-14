namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record CoViewDto : ActionViewDto
{
    [Display(Name = "Received from facility")]
    public DateOnly? ReceivedFromFacility { get; init; }

    [Display(Name = "Executed")]
    public DateOnly? ExecutedDate { get; init; }

    [Display(Name = "Received from Director's Office")]
    public DateOnly? ReceivedFromDirectorsOffice { get; init; }

    [Display(Name = "Resolved")]
    public DateOnly? ResolvedDate { get; init; }

    [Display(Name = "Order number")]
    public string? OrderNumber { get; init; }

    [Display(Name = "Penalty assessed")]
    public decimal? PenaltyAmount { get; init; }

    [Display(Name = "Penalty comment")]
    public string? PenaltyComment { get; init; }

    public bool StipulatedPenaltiesDefined { get; init; }

    [Display(Name = "Stipulated penalties")]
    public ICollection<StipulatedPenaltyViewDto> StipulatedPenalties { get; } = [];
}
