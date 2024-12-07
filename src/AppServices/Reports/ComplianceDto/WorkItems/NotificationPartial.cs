using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Reports.ComplianceDto.WorkItems;

public record NotificationPartial
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Date received")]
    public DateTime ReceivedDate { get; init; }

    public PersonName Reviewer { get; set; }

    [Display(Name = "Notification type")]
    public string Type { get; init; } = "";

    public string Comments { get; init; } = "";
}
