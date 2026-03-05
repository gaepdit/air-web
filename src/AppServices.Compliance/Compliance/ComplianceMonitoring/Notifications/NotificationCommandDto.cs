using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Core.Utilities;
using AirWeb.Domain.Core.Data.DataAttributes;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Notifications;

public abstract record NotificationCommandDto : ComplianceWorkCommandDto, INotificationCommandDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received")]
    [MaxDate]
    public DateOnly ReceivedDate { get; [UsedImplicitly] init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Due Date")]
    [MaxDate(365)]
    public DateOnly? DueDate { get; [UsedImplicitly] init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Sent By Facility")]
    [MaxDate]
    public DateOnly? SentDate { get; [UsedImplicitly] init; }

    [Required]
    [Display(Name = "Notification Type")]
    public Guid? NotificationTypeId { get; [UsedImplicitly] init; }

    [Display(Name = "Follow-Up Action Taken")]
    public bool FollowupTaken { get; [UsedImplicitly] init; }
}
