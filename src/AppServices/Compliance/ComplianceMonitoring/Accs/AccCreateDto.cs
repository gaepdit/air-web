using AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Command;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Accs;

public record AccCreateDto : AccCommandDto, IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
