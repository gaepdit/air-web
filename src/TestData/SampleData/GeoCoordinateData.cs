using AirWeb.Core.Entities.ValueObjects;

namespace AirWeb.TestData.SampleData;

public static class GeoCoordinateData
{
    private static readonly GeoCoordinates[] GeoCoordinatesData =
    [
        new(32.713m, -83.495m),
        new(33.749308m, -84.386386m),
        new(30.811765m, -82.274658m),
        new(31.485044m, -83.1199544m),
        new(34.839663m, -85.4836493m),
    ];

    public static GeoCoordinates GetRandomGeoCoordinates() =>
        GeoCoordinatesData[Random.Shared.Next(GeoCoordinatesData.Length)];
}
