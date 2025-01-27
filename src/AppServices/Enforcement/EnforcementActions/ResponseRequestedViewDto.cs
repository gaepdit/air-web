namespace AirWeb.AppServices.Enforcement.EnforcementActions;

public record ResponseRequestedViewDto : ActionViewDto
{
    [Display(Name = "Response requested")]
    public bool ResponseRequested { get; init; }

    [Display(Name = "Response received")]
    public DateOnly? ResponseReceived { get; init; }
}
