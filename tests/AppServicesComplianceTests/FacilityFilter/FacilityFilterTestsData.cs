using AirWeb.Domain.Compliance.Facility;
using IaipDataService.Facilities;
using JetBrains.Annotations;

namespace AppServicesTests.FacilityFilter;

internal static class FacilityFilterTestsData
{
    public record SearchEntity : IFacilityId
    {
        public int Id { [UsedImplicitly] get; init; }
        public required string FacilityId { get; init; }
    }

    public static List<SearchEntity> SearchData =>
    [
        new()
        {
            Id = 1,
            FacilityId = (FacilityId)"001-00001",
        },
        new()
        {
            Id = 2,
            FacilityId = (FacilityId)"001-00001",
        },
        new()
        {
            Id = 3,
            FacilityId = (FacilityId)"001-00003",
        },
        new()
        {
            Id = 4,
            FacilityId = (FacilityId)"003-00001",
        },
    ];
}
