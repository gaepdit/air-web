using AirWeb.Domain.Entities.WorkEntries;

namespace AirWeb.AppServices.WorkEntries.CommandDto;

public interface IWorkEntryCreateDto
{
    public string FacilityId { get; }
    public WorkEntryType WorkEntryType { get; }
    public string? ResponsibleStaffId { get; }
    public bool IsClosed { get; }
    public DateOnly? ClosedDate { get; }
    public DateOnly? AcknowledgmentLetterDate { get; }
    public string Notes { get; }
}
