namespace AirWeb.Domain.Core.Entities.ValueObjects;

public record struct DateTimeRange(
    DateTime StartDate,
    DateTime? EndDate
);
