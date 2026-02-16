using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions.ActionProperties;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionQuery;

public record ReviewDto
{
    public DateTime RequestedDate { get; internal init; }
    public string RequestedByFullName { get; internal init; } = null!;
    public string RequestedOfFullName { get; internal init; } = null!;
    public string? ReviewedByFullName { get; internal init; }

    public bool IsCompleted => CompletedDate.HasValue;
    public DateTime? CompletedDate { get; internal init; }

    public ReviewResult? Result { get; internal init; }

    [StringLength(7000)]
    public string? ReviewComments { get; internal init; }
}
