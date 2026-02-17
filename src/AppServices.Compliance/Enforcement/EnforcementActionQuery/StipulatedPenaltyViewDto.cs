namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionQuery;

public record StipulatedPenaltyViewDto
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    public DateOnly ReceivedDate { get; init; }
    public string? Notes { get; init; }
    public bool IsDeleted { get; init; }
}
