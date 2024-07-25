using AirWeb.AppServices.DomainEntities.WorkEntries.WorkEntryDto;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.DomainEntities.WorkEntries.Notifications;

public record NotificationUpdateDto : WorkEntryUpdateDto, INotificationCommandDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Due Date")]
    public DateOnly? DueDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date Sent by Facility")]
    public DateOnly? SentDate { get; init; }

    [Required]
    [Display(Name = "Notification Type")]
    public Guid? NotificationTypeId { get; [UsedImplicitly] init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
