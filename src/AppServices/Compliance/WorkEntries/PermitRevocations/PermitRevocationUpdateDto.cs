namespace AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;

public record PermitRevocationUpdateDto : PermitRevocationCommandDto
{
    public bool IsClosed { get; init; }
    public bool IsDeleted { get; init; }
}
