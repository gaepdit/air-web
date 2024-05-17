namespace AirWeb.Domain.ValueObjects;

public record struct DateRange
(
    DateOnly StartDate,
    DateOnly? EndDate
);
