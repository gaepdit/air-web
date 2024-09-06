using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.ExternalEntities;

internal static class FacilityData
{
    private static IEnumerable<Facility> FacilitySeedItems =>
    [
        new("001-00001")
        {
            CompanyName = SampleText.GetRandomText(SampleText.TextLength.Phrase),
            Description = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
            FacilityAddress = AddressData.GetRandomAddress(),
            County = DomainData.GetRandomCounty(),
            GeoCoordinates = GeoCoordinateData.GetRandomGeoCoordinates(),
            OperatingStatusCode = FacilityOperatingStatus.O,
            ClassificationCode = FacilityClassification.A,
        },
        new("002-00002")
        {
            CompanyName = SampleText.GetRandomText(SampleText.TextLength.Phrase),
            Description = string.Empty,
            FacilityAddress = AddressData.GetRandomAddress(),
            County = DomainData.GetRandomCounty(),
            GeoCoordinates = GeoCoordinateData.GetRandomGeoCoordinates(),
            OperatingStatusCode = FacilityOperatingStatus.O,
            ClassificationCode = FacilityClassification.S,
        },
        new("003-00003")
        {
            CompanyName = SampleText.GetRandomText(SampleText.TextLength.Phrase),
            Description = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
            FacilityAddress = AddressData.GetRandomAddress(),
            County = DomainData.GetRandomCounty(),
            GeoCoordinates = GeoCoordinateData.GetRandomGeoCoordinates(),
            OperatingStatusCode = FacilityOperatingStatus.X,
            ClassificationCode = FacilityClassification.A,
        },
        new("004-00004")
        {
            CompanyName = SampleText.GetRandomText(SampleText.TextLength.Phrase),
            Description = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
            FacilityAddress = AddressData.GetRandomAddress(),
            County = DomainData.GetRandomCounty(),
            GeoCoordinates = GeoCoordinateData.GetRandomGeoCoordinates(),
            OperatingStatusCode = FacilityOperatingStatus.C,
            ClassificationCode = FacilityClassification.B,
        },
    ];

    private static List<Facility>? _facilities;

    public static List<Facility> GetData
    {
        get
        {
            if (_facilities is not null) return _facilities;
            _facilities = FacilitySeedItems.ToList();
            return _facilities;
        }
    }

    private static Dictionary<string, string>? _facilityNames;

    public static Dictionary<string, string> FacilityNames
    {
        get
        {
            if (_facilityNames is not null) return _facilityNames;
            _facilityNames = GetData.ToDictionary(facility => facility.Id.ToString(), facility => facility.CompanyName);
            return _facilityNames;
        }
    }
}
