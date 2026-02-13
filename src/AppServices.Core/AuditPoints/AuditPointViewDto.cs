namespace AirWeb.AppServices.Core.AuditPoints;

public record AuditPointViewDto
{
    public string What { get; init; } = null!;
    public string? WhoFullName { get; init; }
    public DateTimeOffset When { get; init; }
    public string? MoreInfo { get; init; }
}
