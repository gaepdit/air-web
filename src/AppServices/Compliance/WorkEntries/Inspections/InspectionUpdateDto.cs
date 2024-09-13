namespace AirWeb.AppServices.Compliance.WorkEntries.Inspections;

public record InspectionUpdateDto : InspectionCommandDto
{
    public bool IsDeleted { get; init; }
    public bool IsClosed { get; init; }
}
