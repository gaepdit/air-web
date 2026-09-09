using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.PermitRevocations;

public record PermitRevocationCreateDto : PermitRevocationCommandDto, IComplianceWorkCreateDto
{
    public string? FacilityId { get; set; }
    public int? CaseFileId => null;
}
