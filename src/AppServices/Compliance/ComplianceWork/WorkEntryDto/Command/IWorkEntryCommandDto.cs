namespace AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;

public interface IWorkEntryCommandDto
{
    // Data
    public string? ResponsibleStaffId { get; }
    public DateOnly? AcknowledgmentLetterDate { get; }
    public string? Notes { get; }
}
