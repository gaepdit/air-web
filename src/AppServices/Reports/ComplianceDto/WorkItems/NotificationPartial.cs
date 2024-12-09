using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Reports.ComplianceDto.WorkItems;

public record NotificationPartial
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Date received")]
    public DateOnly ReceivedDate { get; init; }

    public PersonName Reviewer { get; init; }

    [Display(Name = "Notification type")]
    public string Type { get; init; } = "";

    public string Notes { get; init; } = "";
}
