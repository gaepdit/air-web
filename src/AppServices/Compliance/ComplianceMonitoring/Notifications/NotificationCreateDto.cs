using AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Command;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Notifications;

public record NotificationCreateDto : NotificationCommandDto, IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
