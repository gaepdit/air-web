namespace AirWeb.AppServices.WorkEntries.CommandDto;

public interface IWorkEntryUpdateDto
{
    // Authorization handler assist properties
    public bool IsDeleted { get; init; }

    // Data
    public string? ResponsibleStaffId { get; }
    public bool IsClosed { get; }
    public DateOnly? ClosedDate { get; }
    public DateOnly? AcknowledgmentLetterDate { get; }
    public string Notes { get; }
}
