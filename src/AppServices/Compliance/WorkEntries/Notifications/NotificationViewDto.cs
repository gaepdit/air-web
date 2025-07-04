﻿using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.AppServices.NamedEntities.NotificationTypes;

namespace AirWeb.AppServices.Compliance.WorkEntries.Notifications;

public record NotificationViewDto : WorkEntryViewDto
{
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    [Display(Name = "Due Date")]
    public DateOnly? DueDate { get; init; }

    [Display(Name = "Date Sent By Facility")]
    public DateOnly? SentDate { get; init; }

    [Display(Name = "Notification Type")]
    public required NotificationTypeViewDto NotificationType { get; init; }

    [Display(Name = "Follow-Up Action Taken")]
    public bool FollowupTaken { get; init; }
}
