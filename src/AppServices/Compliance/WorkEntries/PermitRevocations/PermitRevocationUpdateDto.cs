using AirWeb.AppServices.CommonInterfaces;

namespace AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;

public record PermitRevocationUpdateDto : PermitRevocationCommandDto, IIsClosedAndIsDeleted
{
    public bool IsClosed { get; init; }
    public bool IsDeleted { get; init; }
}
