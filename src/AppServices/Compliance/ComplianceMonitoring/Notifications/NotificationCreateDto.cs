using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Notifications;

public record NotificationCreateDto : NotificationCommandDto, IComplianceWorkCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
