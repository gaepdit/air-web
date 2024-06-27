using AirWeb.Domain.Entities.Offices;

namespace AirWeb.TestData.Entities;

internal static class OfficeData
{
    private static IEnumerable<Office> OfficeSeedItems =>
    [
        new Office(new Guid("10000000-0000-0000-0000-000000000011"), "Branch Office"),
        new Office(new Guid("10000000-0000-0000-0000-000000000012"), "District Office"),
        new Office(new Guid("10000000-0000-0000-0000-000000000013"), "Region Office"),
        new Office(new Guid("10000000-0000-0000-0000-000000000014"), "Closed Office") { Active = false },
    ];

    private static IEnumerable<Office>? _offices;

    public static IEnumerable<Office> GetData
    {
        get
        {
            if (_offices is not null) return _offices;
            _offices = OfficeSeedItems.ToList();
            return _offices;
        }
    }

    public static void ClearData() => _offices = null;
}
