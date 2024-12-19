using AirWeb.AppServices.CommonInterfaces;

namespace AirWeb.AppServices.Compliance.WorkEntries.Inspections;

public record InspectionUpdateDto : InspectionCommandDto, IIsClosedAndIsDeleted
{
    public bool IsDeleted { get; init; }
    public bool IsClosed { get; init; }
}
