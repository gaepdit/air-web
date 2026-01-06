using AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Command;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Inspections;

public record InspectionCreateDto : InspectionCommandDto, IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }

    public bool IsRmpInspection { get; init; }
}
