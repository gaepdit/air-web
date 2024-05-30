using GaEpd.AppLibrary.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AirWeb.Domain.ValueObjects;

[Owned]
public record GeoCoordinates : ValueObject
{
    public decimal Latitude { get; [UsedImplicitly] init; }
    public decimal Longitude { get; [UsedImplicitly] init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}
