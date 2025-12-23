namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record ProposedCoViewDto : ReportableActionViewDto
{
    [Display(Name = "Response Received")]
    public DateOnly? ResponseReceived { get; init; }
}
