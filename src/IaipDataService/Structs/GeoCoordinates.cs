namespace IaipDataService.Structs;

public readonly record struct GeoCoordinates(decimal Latitude, decimal Longitude)
{
    public string GoogleMapsUrl => $"https://www.google.com/maps/search/?api=1&query={Latitude}%2C{Longitude}";
    public string AppleMapsUrl => $"https://maps.apple.com/?q={Latitude}%2C{Longitude}";
}
