using AirWeb.Domain.ValueObjects;
using GaEpd.AppLibrary.Extensions;

namespace AirWeb.Domain.Entities.Facilities;

public record Facility
{
    public FacilityId Id { get; init; } = default!;

    // Description
    public string CompanyName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;

    // Location
    public Address FacilityAddress { get; init; } = default!;
    public string County { get; init; } = string.Empty;
    public GeoCoordinates? GeoCoordinates { get; init; }

    // Operating status
    public FacilityOperatingStatus OperatingStatusCode { get; [UsedImplicitly] init; }
    public string OperatingStatus => OperatingStatusCode.GetDescription();

    // Classifications
    public FacilityClassification ClassificationCode { get; [UsedImplicitly] init; }
    public string Classification => ClassificationCode.GetDescription();
}
