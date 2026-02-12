namespace AirWeb.Core.Entities.ValueObjects;

public record struct DateRange(
    DateOnly StartDate,
    DateOnly? EndDate
);
