﻿using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.ExternalEntities.Facilities;

public record Facility
{
    public Facility() { }
    public Facility(FacilityId id) => Id = id;
    public Facility(string id) => Id = (FacilityId)id;

    [Key]
    [Column(TypeName = "nvarchar(9)")]
    public FacilityId Id { get; init; } = default!;

    // Description
    public string CompanyName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;

    // Location
    public Address FacilityAddress { get; init; } = default!;
    public string County { get; init; } = string.Empty;
    public GeoCoordinates? GeoCoordinates { get; init; }

    // Status
    public FacilityOperatingStatus OperatingStatusCode { get; [UsedImplicitly] init; }
    public FacilityClassification ClassificationCode { get; [UsedImplicitly] init; }
}
