using MyApp.Domain.Entities.EntryActions;
using MyApp.TestData.Constants;

namespace MyApp.TestData;

internal static class EntryActionData
{
    private static IEnumerable<EntryAction> EntryActionSeedItems => new List<EntryAction>
    {
        new(new Guid("30000000-0000-0000-0000-000000000000"), // 0
            WorkEntryData.GetData.ElementAt(0))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-3).Date),
            Comments = $"Email: {TextData.ValidEmail} & Phone: {TextData.ValidPhoneNumber}",
        },
        new(new Guid("30000000-0000-0000-0000-000000000001"), // 1
            WorkEntryData.GetData.ElementAt(0))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
            Comments = TextData.EmojiWord,
        },
        new(new Guid("30000000-0000-0000-0000-000000000002"), // 2
            WorkEntryData.GetData.ElementAt(0))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
            Comments = "Deleted action on closed entry",
        },
        new(new Guid("30000000-0000-0000-0000-000000000003"), // 3
            WorkEntryData.GetData.ElementAt(3))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-1).Date),
            Comments = "Action on a deleted entry",
        },
        new(new Guid("30000000-0000-0000-0000-000000000004"), // 4
            WorkEntryData.GetData.ElementAt(5))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
            Comments = "Action on open entry",
        },
        new(new Guid("30000000-0000-0000-0000-000000000005"), // 5
            WorkEntryData.GetData.ElementAt(5))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-3).Date),
            Comments = "Deleted action on open entry",
        },
        new(new Guid("30000000-0000-0000-0000-000000000006"), // 6
            WorkEntryData.GetData.ElementAt(3))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-3).Date),
            Comments = "Deleted action on deleted entry",
        },
    };

    private static List<EntryAction>? _entryActions;

    public static IEnumerable<EntryAction> GetData
    {
        get
        {
            if (_entryActions is not null) return _entryActions;

            _entryActions = EntryActionSeedItems.ToList();
            _entryActions[2].SetDeleted("00000000-0000-0000-0000-000000000001");
            _entryActions[5].SetDeleted("00000000-0000-0000-0000-000000000001");
            _entryActions[6].SetDeleted("00000000-0000-0000-0000-000000000001");
            return _entryActions;
        }
    }

    public static void ClearData() => _entryActions = null;
}
