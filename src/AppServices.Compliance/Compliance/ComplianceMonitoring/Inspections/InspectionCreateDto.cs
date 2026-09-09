using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Inspections;

public record InspectionCreateDto : InspectionCommandDto, IComplianceWorkCreateDto
{
    public string? FacilityId { get; set; }
    public int? CaseFileId { get; init; }
    public bool IsRmpInspection { get; init; }
}
