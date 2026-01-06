namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

public interface IComplianceWorkCommandDto
{
    // Data
    public string? ResponsibleStaffId { get; }
    public DateOnly? AcknowledgmentLetterDate { get; }
    public string? Notes { get; }
}
