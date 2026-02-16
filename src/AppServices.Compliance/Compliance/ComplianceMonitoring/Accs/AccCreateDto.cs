using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Accs;

public record AccCreateDto : AccCommandDto, IComplianceWorkCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
