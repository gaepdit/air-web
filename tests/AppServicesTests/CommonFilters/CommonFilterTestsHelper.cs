using AirWeb.Domain.BaseEntities.Interfaces;
using IaipDataService.Facilities;
using JetBrains.Annotations;

namespace AppServicesTests.CommonFilters;

internal static class CommonFilterTestsHelper
{
    public record SearchEntity : IFacilityId, IIsDeleted, INotes
    {
        public int Id { [UsedImplicitly] get; init; }
        public required string FacilityId { get; init; }
        public bool IsDeleted { get; init; }
        public string? Notes { get; init; }
    }

    public static List<SearchEntity> SearchData =>
    [
        new()
        {
            Id = 1,
            FacilityId = (FacilityId)"001-00001",
            IsDeleted = false,
            Notes = null,
        },
        new()
        {
            Id = 2,
            IsDeleted = true,
            FacilityId = (FacilityId)"001-00001",
            Notes = "abc",
        },
        new()
        {
            Id = 1,
            FacilityId = (FacilityId)"001-00003",
            IsDeleted = false,
            Notes = "abcd",
        },
        new()
        {
            Id = 2,
            IsDeleted = true,
            FacilityId = (FacilityId)"003-00001",
            Notes = "",
        },
    ];
}
