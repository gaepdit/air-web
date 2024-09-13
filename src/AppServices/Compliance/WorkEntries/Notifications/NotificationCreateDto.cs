using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.Notifications;

public record NotificationCreateDto : NotificationCommandDto, IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
