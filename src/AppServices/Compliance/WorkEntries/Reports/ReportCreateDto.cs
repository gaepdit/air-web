using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.Reports;

public record ReportCreateDto : ReportCommandDto, IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
