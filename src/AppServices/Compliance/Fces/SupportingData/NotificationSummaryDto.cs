﻿using AirWeb.AppServices.Lookups.NotificationTypes;
using AirWeb.AppServices.Staff.Dto;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record NotificationSummaryDto : BaseSummaryDto
{
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    [Display(Name = "Notification Type")]
    public required NotificationTypeViewDto NotificationType { get; init; }

    [Display(Name = "Reviewer")]
    public StaffViewDto? ResponsibleStaff { get; init; }
}
