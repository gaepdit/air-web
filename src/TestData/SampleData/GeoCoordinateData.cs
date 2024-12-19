using AirWeb.Domain.ValueObjects;

namespace AirWeb.TestData.SampleData;

public static class GeoCoordinateData
{
    private static readonly GeoCoordinates[] GeoCoordinatesData =
    [
        new GeoCoordinates(32.713m, -83.495m),
        new GeoCoordinates(33.749308m, -84.386386m),
        new GeoCoordinates(30.811765m, -82.274658m),
        new GeoCoordinates(31.485044m, -83.1199544m),
        new GeoCoordinates(34.839663m, -85.4836493m),
    ];

    public static GeoCoordinates GetRandomGeoCoordinates() =>
        GeoCoordinatesData[Random.Shared.Next(GeoCoordinatesData.Length)];
}
