namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;

public interface IWorkEntryCreateDto
{
    public string? FacilityId { get; }
    public string? ResponsibleStaffId { get; }
    public DateOnly? AcknowledgmentLetterDate { get; }
    public string Notes { get; }
}
