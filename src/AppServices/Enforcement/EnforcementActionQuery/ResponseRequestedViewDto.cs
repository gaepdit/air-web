namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record ResponseRequestedViewDto : ActionViewDto
{
    [Display(Name = "Response requested")]
    public bool ResponseRequested { get; init; }

    [Display(Name = "Response received")]
    public DateOnly? ResponseReceived { get; init; }

    public bool IsResponseReceived => ResponseReceived != null;

    [Display(Name = "Response received")]
    public string? ResponseComment { get; init; }
}
