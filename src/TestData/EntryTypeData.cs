using MyApp.Domain.Entities.EntryTypes;

namespace MyApp.TestData;

internal static class EntryTypeData
{
    private static IEnumerable<EntryType> EntryTypeSeedItems => new List<EntryType>
    {
        new(new Guid("20000000-0000-0000-0000-000000000000"), "Entry Type Zero"), // 0
        new(new Guid("20000000-0000-0000-0000-000000000001"), "Entry Type One"), // 1
        new(new Guid("20000000-0000-0000-0000-000000000002"), "Entry Type Two"), // 2
        new(new Guid("20000000-0000-0000-0000-000000000003"), "Inactive Entry Type") { Active = false }, // 3
    };

    private static IEnumerable<EntryType>? _entryTypes;

    public static IEnumerable<EntryType> GetData
    {
        get
        {
            if (_entryTypes is not null) return _entryTypes;
            _entryTypes = EntryTypeSeedItems;
            return _entryTypes;
        }
    }

    public static void ClearData() => _entryTypes = null;
}
