using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.EnforcementEntities.ActionProperties;

namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record ReviewDto
{
    public DateOnly RequestedDate { get; internal init; }
    public StaffViewDto RequestedTo { get; internal init; } = null!;
    public StaffViewDto? ReviewedBy { get; internal init; }
    public bool IsCompleted => CompletedDate.HasValue;
    public DateOnly? CompletedDate { get; internal set; }

    [StringLength(11)]
    public ReviewResult? Result { get; internal set; }

    [StringLength(7000)]
    public string? ReviewComments { get; internal set; }
}
