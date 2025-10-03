using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;

namespace AirWeb.AppServices.Compliance.ComplianceWork.Inspections;

public record InspectionCreateDto : InspectionCommandDto, IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }

    public bool IsRmpInspection { get; init; }
}
