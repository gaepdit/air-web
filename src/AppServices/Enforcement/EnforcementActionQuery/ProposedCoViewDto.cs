namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record ProposedCoViewDto : ActionViewDto
{
    [Display(Name = "Response Received")]
    public DateOnly? ResponseReceived { get; init; }
}
