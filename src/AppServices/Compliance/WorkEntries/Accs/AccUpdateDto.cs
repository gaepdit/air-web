using AirWeb.AppServices.CommonInterfaces;

namespace AirWeb.AppServices.Compliance.WorkEntries.Accs;

public record AccUpdateDto : AccCommandDto, IIsClosedAndIsDeleted
{
    public bool IsClosed { get; init; }
    public bool IsDeleted { get; init; }
}
