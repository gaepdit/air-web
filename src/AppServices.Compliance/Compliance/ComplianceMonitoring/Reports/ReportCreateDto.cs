using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Reports;

public record ReportCreateDto : ReportCommandDto, IComplianceWorkCreateDto
{
    public string? FacilityId { get; set; }
    public int? CaseFileId { get; init; }
}
