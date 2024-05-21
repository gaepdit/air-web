using AirWeb.Domain.Entities.WorkEntries;

namespace AirWeb.AppServices.WorkEntries.BaseCommandDto;

public interface IWorkEntryCreateDto
{
    public string FacilityId { get; }
    public WorkEntryType WorkEntryType { get; }
    public string? ResponsibleStaffId { get; }
    public DateOnly? AcknowledgmentLetterDate { get; }
    public string Notes { get; }
}
