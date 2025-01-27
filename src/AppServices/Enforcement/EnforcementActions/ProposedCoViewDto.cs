namespace AirWeb.AppServices.Enforcement.EnforcementActions;

public record ProposedCoViewDto : ActionViewDto
{
    [Display(Name = "Response received")]
    public DateOnly? ResponseReceived { get; init; }
}
