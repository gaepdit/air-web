using IaipDataService.Facilities;
using IaipDataService.Permits;

namespace IaipDataService.TestData;

public static class PermitData
{
    private const string Facility1 = "001-00001";

    private static List<PermitSummary> SeedItems =>
    [
        new()
        {
            FileType = PermitSummary.FileTypes.Sip,
            FacilityId = Facility1,
            FacilityName = FacilityData.GetFacility((FacilityId)Facility1).Name,
            PermitNumber = "9999-001-0001-V-01-0",
            IssuanceDate = DateTime.Today.AddMonths(-5),
            OtherPermit = "OP-0001",
            OtherNarrative = "ON-0001",
        },
        new()
        {
            FileType = PermitSummary.FileTypes.Psd,
            FacilityId = Facility1,
            FacilityName = FacilityData.GetFacility((FacilityId)Facility1).Name,
            PermitNumber = "9999-001-0001-P-01-1",
            IssuanceDate = DateTime.Today.AddMonths(-4),
            PsdFinal = "PI-0002",
            PsdNarrative = "PT-0002",
            PsdPrelim = "PP-0002",
            PsdFinalDet = "PF-0002",
            PsdAppSum = "PA-0002",
        },
        new()
        {
            FileType = PermitSummary.FileTypes.TitleV,
            FacilityId = Facility1,
            FacilityName = FacilityData.GetFacility((FacilityId)Facility1).Name,
            PermitNumber = "9999-001-0001-V-02-0",
            IssuanceDate = DateTime.Today.AddMonths(-2),
            VFinal = "VF-0003",
            VNarrative = "VN-0003",
        },
        new()
        {
            FileType = PermitSummary.FileTypes.Sip,
            FacilityId = "121-00021",
            FacilityName = FacilityData.GetFacility((FacilityId)"121-00021").Name,
            PermitNumber = "9999-121-0021-V-01-0",
            IssuanceDate = DateTime.Today.AddYears(-5),
            OtherPermit = "OP-0004",
            OtherNarrative = "ON-0004",
        },
    ];

    public static List<PermitSummary> GetData
    {
        get
        {
            if (field is not null) return field;
            field = SeedItems;
            return field;
        }
    }
}
