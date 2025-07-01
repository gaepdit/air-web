using AirWeb.AppServices.DtoInterfaces;

namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record ResponseRequestedViewDto : ActionViewDto, IResponseRequested
{
    [Display(Name = "Response Requested")]
    public bool ResponseRequested { get; init; }

    [Display(Name = "Response Received")]
    public DateOnly? ResponseReceived { get; init; }

    public bool IsResponseReceived => ResponseReceived != null;

    [Display(Name = "Response Received")]
    public string? ResponseComment { get; init; }
}
