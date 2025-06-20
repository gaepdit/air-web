using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.BaseEntities.Interfaces;
using IaipDataService.Facilities;
using JetBrains.Annotations;

namespace AppServicesTests.CommonFilters;

internal static class CommonFilterTestsHelper
{
    public record SearchEntity : IFacilityId, IIsDeleted, IIsClosed, INotes
    {
        public int Id { [UsedImplicitly] get; init; }
        public required string FacilityId { get; init; }
        public bool IsDeleted { get; init; }
        public bool IsClosed { get; init; }
        public string? Notes { get; init; }
    }

    public static List<SearchEntity> SearchData =>
    [
        new()
        {
            Id = 1,
            FacilityId = (FacilityId)"001-00001",
            IsDeleted = false,
            IsClosed = true,
            Notes = null,
        },
        new()
        {
            Id = 2,
            FacilityId = (FacilityId)"001-00001",
            IsDeleted = true,
            IsClosed = true,
            Notes = "abc",
        },
        new()
        {
            Id = 1,
            FacilityId = (FacilityId)"001-00003",
            IsDeleted = false,
            IsClosed = false,
            Notes = "abc.d",
        },
        new()
        {
            Id = 2,
            FacilityId = (FacilityId)"003-00001",
            IsDeleted = true,
            IsClosed = false,
            Notes = "",
        },
    ];
}
