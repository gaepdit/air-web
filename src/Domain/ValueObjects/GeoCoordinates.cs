﻿using GaEpd.AppLibrary.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AirWeb.Domain.ValueObjects;

[Owned]
public record GeoCoordinates(
    [property: Column(TypeName = "decimal(8, 6)")] decimal Latitude,
    [property: Column(TypeName = "decimal(9, 6)")] decimal Longitude) : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}
