using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.PermitRevocations;

public record PermitRevocationCreateDto : PermitRevocationCommandDto, IComplianceWorkCreateDto
{
    [Required]
    public string? FacilityId { get; set; }

    public int? CaseFileId => null;
}
