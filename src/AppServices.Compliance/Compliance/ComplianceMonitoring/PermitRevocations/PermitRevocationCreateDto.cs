using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.PermitRevocations;

public record PermitRevocationCreateDto : PermitRevocationCommandDto, IComplianceWorkCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
