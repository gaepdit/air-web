﻿using AirWeb.AppServices.DomainEntities.WorkEntries.BaseWorkEntryDto;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.DomainEntities.WorkEntries.Notifications;

public record NotificationCreateDto : BaseWorkEntryCreateDto, INotificationCommandDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; [UsedImplicitly] init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Due Date")]
    public DateOnly? DueDate { get; [UsedImplicitly] init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date Sent by Facility")]
    public DateOnly? SentDate { get; [UsedImplicitly] init; }

    [Required]
    [Display(Name = "Notification Type")]
    public Guid? NotificationTypeId { get; [UsedImplicitly] init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; [UsedImplicitly] init; }
}
