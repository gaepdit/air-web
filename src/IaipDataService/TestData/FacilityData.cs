using IaipDataService.Facilities;
using IaipDataService.Structs;

namespace IaipDataService.TestData;

public static class FacilityData
{
    public static Facility GetRandomFacility() => GetData[Random.Shared.Next(GetData.Count)];
    public static Facility GetFacility(FacilityId id) => GetData.Single(facility => facility.Id == id);
    public static Facility GetFacility(string id) => GetData.Single(facility => facility.Id == id);

    private static readonly RegulatoryData SampleRegulatoryData = new()
    {
        OperatingStatusCode = FacilityOperatingStatus.O,
        ClassificationCode = FacilityClassification.SM,
        CmsClassificationCode = FacilityCmsClassification.S,
        Sic = "1234",
        Naics = "123456",
        RmpId = "1234-5678-9012",
        OwnershipType = "Federal Facility (U.S. Government)",
        StartupDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local),
        PermitRevocationDate = null,
        Pollutants =
        [
            new Pollutant("10193", "Carbon Monoxide"),
            new Pollutant("300000005", "Nitrogen Dioxide"),
            new Pollutant("300000242", "Total HAP"),
            new Pollutant("300000301", "Fugitive Dust"),
            new Pollutant("300000319", "Particulate Matter < 10 um"),
            new Pollutant("300000329", "Facility Wide"),
        ],
        AirPrograms = [AirProgram.SIP, AirProgram.NSPS],
        ProgramClassifications = [AirProgramClassification.NsrMajor, AirProgramClassification.HapMajor],
        OneHourOzoneNonattainment = OneHourOzoneNonattainmentStatus.No,
        EightHourOzoneNonattainment = EightHourOzoneNonattainmentStatus.None,
        PmFineNonattainment = PmFineNonattainmentStatus.None,
        NspsFeeExempt = true,
    };

    private static IEnumerable<Facility> FacilitySeedItems =>
    [
        new("00100001")
        {
            Name = "Apple Corp",
            County = "Appling",
            Description = "Apples and more",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = null,
                City = "Atlantis",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
            RegulatoryData = new RegulatoryData
            {
                OperatingStatusCode = FacilityOperatingStatus.O,
                ClassificationCode = FacilityClassification.A,
                CmsClassificationCode = FacilityCmsClassification.A,
                Sic = "1234",
                Naics = "123456",
                RmpId = null,
                OwnershipType = null,
                StartupDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local),
                PermitRevocationDate = null,
                Pollutants =
                [
                    new Pollutant("10193", "Carbon Monoxide"),
                    new Pollutant("10461", "Sulfur Dioxide"),
                    new Pollutant("300000005", "Nitrogen Dioxide"),
                    new Pollutant("300000236", "Visible Emissions"),
                    new Pollutant("300000242", "Total HAP"),
                    new Pollutant("300000243", "Volatile Organic Compounds"),
                    new Pollutant("300000301", "Fugitive Dust"),
                    new Pollutant("300000302", "Fugitive Emissions"),
                    new Pollutant("300000319", "Particulate Matter < 10 um"),
                    new Pollutant("300000322", "Total Particulate Matter"),
                    new Pollutant("300000328", "Administration"),
                    new Pollutant("300000329", "Facility Wide"),
                    new Pollutant("300000330", "Other Emissions Other than road based"),
                ],
                AirPrograms = [AirProgram.SIP, AirProgram.MACT],
                ProgramClassifications = [AirProgramClassification.NsrMajor],
                OneHourOzoneNonattainment = OneHourOzoneNonattainmentStatus.Contribute,
                EightHourOzoneNonattainment = EightHourOzoneNonattainmentStatus.Atlanta,
                PmFineNonattainment = PmFineNonattainmentStatus.Atlanta,
                NspsFeeExempt = false,
            },
        },
        new("12100021")
        {
            Name = "Banana Corp",
            County = "Bibb",
            Description = "Bananas and more",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite B",
                City = "Bedford Falls",
                State = "GA",
                PostalCode = "30000",
            },
            RegulatoryData = SampleRegulatoryData,
        },
        new("05100149")
        {
            Name = "Cranberry Corp",
            County = "Clay",
            Description = "Cranberries and more",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = null,
                City = "Coruscant",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
            RegulatoryData = new RegulatoryData
            {
                OperatingStatusCode = FacilityOperatingStatus.X,
                ClassificationCode = FacilityClassification.Unspecified,
                CmsClassificationCode = FacilityCmsClassification.Unspecified,
                Sic = "1234",
                Naics = "123456",
                StartupDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local),
                PermitRevocationDate = new DateTime(2020, 2, 2, 0, 0, 0, DateTimeKind.Local),
                AirPrograms = [AirProgram.SIP],
                ProgramClassifications = [AirProgramClassification.HapMajor],
                OneHourOzoneNonattainment = OneHourOzoneNonattainmentStatus.No,
                EightHourOzoneNonattainment = EightHourOzoneNonattainmentStatus.None,
                PmFineNonattainment = PmFineNonattainmentStatus.None,
            },
        },
        new("17900001")
        {
            Name = "Date Corp",
            County = "Dade",
            Description = "Dates and times",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite D",
                City = "Duckburg",
                State = "GA",
                PostalCode = "30000",
            },
            RegulatoryData = SampleRegulatoryData,
        },
        new("05900071")
        {
            Name = "Elderberry Inc.",
            County = "Early",
            Description = "Your mother was a hamster and your father smelt of elderberries!",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = null,
                City = "Emerald City",
                State = "GA",
                PostalCode = "30000",
            },
            RegulatoryData = SampleRegulatoryData,
        },
        new("05700040")
        {
            Name = "Fruit Inc.",
            County = "Floyd",
            Description = "Nothing but fruit",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite F",
                City = "Fer-de-Lance",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
            RegulatoryData = SampleRegulatoryData,
        },
        new("00100005")
        {
            Name = "Guava Inc.",
            County = "Glynn",
            Description = "Guavalicious",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite G",
                City = "Gnu York",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
            RegulatoryData = new RegulatoryData
            {
                OperatingStatusCode = FacilityOperatingStatus.O,
                ClassificationCode = FacilityClassification.SM,
                CmsClassificationCode = FacilityCmsClassification.S,
                Sic = "1234",
                Naics = "123456",
                RmpId = "1234-5678-9012",
                OwnershipType = "Federal Facility (U.S. Government)",
                StartupDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local),
                PermitRevocationDate = null,
                Pollutants = [new Pollutant("300000329", "Facility Wide")],
                AirPrograms = [AirProgram.SIP, AirProgram.NSPS],
                ProgramClassifications = [],
                OneHourOzoneNonattainment = OneHourOzoneNonattainmentStatus.No,
                EightHourOzoneNonattainment = EightHourOzoneNonattainmentStatus.None,
                PmFineNonattainment = PmFineNonattainmentStatus.None,
                NspsFeeExempt = true,
            },
        },
        new("24500002")
        {
            Name = "Huckleberry LLC",
            County = "Hall",
            Description = "Huckleberries & Chuckleberries",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite H",
                City = "Hill Valley",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
            RegulatoryData = SampleRegulatoryData,
        },
        new("07300003")
        {
            Name = "Indian Fig Co.",
            County = "Irwin",
            Description = "Prickly pears",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite I",
                City = "Isthmus City",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
            RegulatoryData = SampleRegulatoryData,
        },
        new("11500021")
        {
            Name = "Juniper Berry Co.",
            County = "Jones",
            Description = "Genièvre",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite J",
                City = "Jump City",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
            RegulatoryData = SampleRegulatoryData,
        },
        new("15300040")
        {
            Name = "Lingonberry LLC",
            County = "Lee",
            Description = "Lingonberries",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite L",
                City = "Lost City of Atlanta",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
            RegulatoryData = SampleRegulatoryData,
        },
        new("30500001")
        {
            Name = "Muscadine Inc.",
            County = "McIntosh",
            Description = "Jellies and Jams",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite M",
                City = "Maycomb",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
            RegulatoryData = SampleRegulatoryData,
        },
        new("31300062")
        {
            Name = "Nectarine Corp.",
            County = "Newton",
            Description = "Nectarines and More",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite N",
                City = "North Haverbrook",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
            RegulatoryData = SampleRegulatoryData,
        },
    ];

    private static List<Facility>? _facilities;

    public static List<Facility> GetData
    {
        get
        {
            if (_facilities is not null) return _facilities;
            _facilities = FacilitySeedItems.ToList();

            foreach (var facility in _facilities)
                facility.NextActionNumber = (ushort)Random.Shared.Next(15_000, ushort.MaxValue / 2);

            return _facilities;
        }
    }
}
