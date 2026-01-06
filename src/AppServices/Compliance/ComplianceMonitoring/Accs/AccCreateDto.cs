using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Accs;

public record AccCreateDto : AccCommandDto, IComplianceWorkCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
