using AirWeb.AppServices.WorkEntries.BaseWorkEntryDto;
using AirWeb.Domain.Entities.WorkEntries;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.WorkEntries.Notifications;

public record NotificationViewDto : BaseWorkEntryViewDto
{
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    [Display(Name = "Due Date")]
    public DateOnly? DueDate { get; init; }

    [Display(Name = "Date Sent by Facility")]
    public DateOnly? SentDate { get; init; }

    [Display(Name = "Notification Type")]
    public string? NotificationType { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
