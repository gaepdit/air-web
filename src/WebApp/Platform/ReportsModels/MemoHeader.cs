namespace AirWeb.WebApp.Platform.ReportsModels;

public record struct MemoHeader
{
    public DateTime? Date { get; init; }
    public string? To { get; init; }
    public string? Through { get; init; }
    public string? From { get; init; }
    public string? Subject { get; init; }
}
