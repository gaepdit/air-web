using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.AppServices.NamedEntities.NotificationTypes;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.Notifications;

public record NotificationViewDto : WorkEntryViewDto
{
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    [Display(Name = "Due Date")]
    public DateOnly? DueDate { get; init; }

    [Display(Name = "Date Sent by Facility")]
    public DateOnly? SentDate { get; init; }

    [Display(Name = "Notification Type")]
    public required NotificationTypeViewDto NotificationType { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
