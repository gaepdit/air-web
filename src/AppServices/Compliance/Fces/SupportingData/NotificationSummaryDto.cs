using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.AppServices.Staff.Dto;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record NotificationSummaryDto
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Comments:")]
    public string Notes { get; init; } = null!;

    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    [Display(Name = "Notification Type")]
    public required NotificationTypeViewDto NotificationType { get; init; }

    [Display(Name = "Reviewer")]
    public StaffViewDto? ResponsibleStaff { get; init; }
}
