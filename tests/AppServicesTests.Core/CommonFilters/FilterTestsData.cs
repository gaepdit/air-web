using AirWeb.Domain.Core.BaseEntities;
using JetBrains.Annotations;

namespace AppServicesTests.Core.CommonFilters;

internal static class FilterTestsData
{
    public record SearchEntity : IIsDeleted, IIsClosed, INotes
    {
        public int Id { [UsedImplicitly] get; init; }
        public bool IsDeleted { get; init; }
        public bool IsClosed { get; init; }
        public string? Notes { get; init; }
    }

    public static List<SearchEntity> SearchData =>
    [
        new()
        {
            Id = 1,
            IsDeleted = false,
            IsClosed = true,
            Notes = null,
        },
        new()
        {
            Id = 2,
            IsDeleted = true,
            IsClosed = true,
            Notes = "abc",
        },
        new()
        {
            Id = 3,
            IsDeleted = false,
            IsClosed = false,
            Notes = "abc.d",
        },
        new()
        {
            Id = 4,
            IsDeleted = true,
            IsClosed = false,
            Notes = "",
        },
    ];
}
