namespace AirWeb.Domain.Core.ValueObjects;

public record struct DateTimeRange(
    DateTime StartDate,
    DateTime? EndDate
);
