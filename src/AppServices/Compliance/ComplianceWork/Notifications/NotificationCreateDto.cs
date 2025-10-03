using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;

namespace AirWeb.AppServices.Compliance.ComplianceWork.Notifications;

public record NotificationCreateDto : NotificationCommandDto, IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
