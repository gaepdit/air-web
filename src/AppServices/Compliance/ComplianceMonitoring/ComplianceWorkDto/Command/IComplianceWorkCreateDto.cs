namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

public interface IComplianceWorkCreateDto : IComplianceWorkCommandDto
{
    public string? FacilityId { get; }
}
