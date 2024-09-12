namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;

public interface IWorkEntryUpdateDto
{
    // Authorization handler assist properties
    public bool IsDeleted { get; }

    // Data
    public string? ResponsibleStaffId { get; }
    public DateOnly? AcknowledgmentLetterDate { get; }
    public string? Notes { get; }
}
