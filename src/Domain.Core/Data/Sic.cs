namespace AirWeb.Domain.Core.Data;

public record Sic
{
    [Key, StringLength(4)]
    public required string Code { get; init; }

    public required string Description { get; init; }
    public bool Active { get; init; } = true;
    public string Display => $"{Code} – {Description}{(Active ? "" : " [Inactive]")}";
}
