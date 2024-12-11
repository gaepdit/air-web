namespace AirWeb.Domain.ValueObjects;

public record struct DateTimeRange(
    DateTime StartDate,
    DateTime? EndDate
);
