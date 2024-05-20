using AirWeb.Domain.Entities.WorkEntries;

namespace AirWeb.AppServices.WorkEntries.QueryDto;

public record WorkEntrySearchResultDto
{
    public int Id { get; init; }
    public DateTimeOffset ReceivedDate { get; init; }
    public WorkEntryStatus Status { get; init; }
    public bool Closed { get; init; }
    public DateTimeOffset? ClosedDate { get; init; }
    public string? EntryTypeName { get; init; }
    public bool IsDeleted { get; init; }
}
