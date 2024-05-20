namespace AirWeb.AppServices.WorkEntries.CommandDto;

public interface IWorkEntryUpdateDto : IWorkEntryCreateDto
{
    // Authorization handler assist properties
    public bool IsClosed { get; init; }
    public bool IsDeleted { get; init; }
}
