using MyApp.Domain.Entities.Offices;

namespace MyApp.TestData;

internal static class OfficeData
{
    private static IEnumerable<Office> OfficeSeedItems => new List<Office>
    {
        new(new Guid("00000000-0000-0000-0000-000000000004"), "Branch Office"),
        new(new Guid("00000000-0000-0000-0000-000000000005"), "District Office"),
        new(new Guid("00000000-0000-0000-0000-000000000006"), "Region Office"),
        new(new Guid("00000000-0000-0000-0000-000000000007"), "Closed Office") { Active = false },
    };

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
