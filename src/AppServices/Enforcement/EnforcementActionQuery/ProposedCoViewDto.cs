namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record ProposedCoViewDto : ActionViewDto
{
    [Display(Name = "Response received")]
    public DateOnly? ResponseReceived { get; init; }
}
