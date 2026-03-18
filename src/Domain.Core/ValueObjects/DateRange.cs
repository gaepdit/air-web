namespace AirWeb.Domain.Core.ValueObjects;

public record struct DateRange(
    DateOnly StartDate,
    DateOnly? EndDate
);
