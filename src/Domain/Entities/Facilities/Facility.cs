namespace AirWeb.Domain.Entities.Facilities;

public record Facility(FacilityId Id)
{
    public string CompanyName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
