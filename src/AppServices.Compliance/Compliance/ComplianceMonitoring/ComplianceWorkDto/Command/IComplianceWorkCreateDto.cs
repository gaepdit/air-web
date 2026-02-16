namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

public interface IComplianceWorkCreateDto : IComplianceWorkCommandDto
{
    public string? FacilityId { get; }
}
