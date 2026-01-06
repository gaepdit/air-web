namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Command;

public interface IWorkEntryCommandDto
{
    // Data
    public string? ResponsibleStaffId { get; }
    public DateOnly? AcknowledgmentLetterDate { get; }
    public string? Notes { get; }
}
