namespace AirWeb.AppServices.Enforcement.EnforcementActions;

public record StipulatedPenaltyViewDto
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    public DateOnly ReceivedDate { get; init; }
    public string? Notes { get; init; }
}
