namespace AirWeb.AppServices.WorkEntries.BaseWorkEntryDto;

public interface IWorkEntryCreateDto
{
    public string FacilityId { get; }
    public string? ResponsibleStaffId { get; }
    public DateOnly? AcknowledgmentLetterDate { get; }
    public string Notes { get; }
}
