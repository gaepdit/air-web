namespace AirWeb.WebApp.Platform.PrintoutModels;

public record struct MemoHeader
{
    public DateOnly? Date { get; init; }
    public string? To { get; init; }
    public string? Through { get; init; }
    public string? From { get; init; }
    public string? Subject { get; init; }
}
