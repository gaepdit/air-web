using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Reports;

public record ReportCreateDto : ReportCommandDto, IComplianceWorkCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
