using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Accs;

public record AccCreateDto : AccCommandDto, IComplianceWorkCreateDto
{
    public string? FacilityId { get; set; }
    public int? CaseFileId { get; init; }
}
