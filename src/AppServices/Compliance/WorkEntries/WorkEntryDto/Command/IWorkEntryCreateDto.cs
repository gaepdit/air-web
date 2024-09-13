namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;

public interface IWorkEntryCreateDto : IWorkEntryCommandDto
{
    public string? FacilityId { get; }
}
