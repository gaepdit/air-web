using MyApp.Domain.Entities.WorkEntries;
using MyApp.TestData.Constants;
using MyApp.TestData.Identity;

namespace MyApp.TestData;

internal static class WorkEntryData
{
    private static IEnumerable<WorkEntry> WorkEntrySeedItems => new List<WorkEntry>
    {
        new(new Guid("10000000-0000-0000-0000-000000000000")) // 0
        {
            Closed = true,
            Status = WorkEntryStatus.Closed,
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            EntryType = EntryTypeData.GetData.ElementAt(0),
            Notes = TextData.Paragraph,
        },
        new(new Guid("10000000-0000-0000-0000-000000000001")) // 1
        {
            Closed = false,
            Status = WorkEntryStatus.Open,
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            ReceivedDate = DateTimeOffset.Now.AddMinutes(30),
            EntryType = EntryTypeData.GetData.ElementAt(0),
        },
        new(new Guid("10000000-0000-0000-0000-000000000002")) // 2
        {
            Closed = true,
            Status = WorkEntryStatus.Closed,
            ReceivedBy = UserData.GetUsers.ElementAt(2),
            EntryType = EntryTypeData.GetData.ElementAt(1),
        },
        new(new Guid("10000000-0000-0000-0000-000000000003")) // 3
        {
            Notes = "Deleted work entry",
            Closed = true,
            Status = WorkEntryStatus.Closed,
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            DeleteComments = TextData.Paragraph,
            EntryType = EntryTypeData.GetData.ElementAt(2),
        },
        new(new Guid("10000000-0000-0000-0000-000000000004")) // 4
        {
            Closed = false,
            Status = WorkEntryStatus.Open,
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            EntryType = null,
        },
        new(new Guid("10000000-0000-0000-0000-000000000005")) // 5
        {
            Closed = false,
            Status = WorkEntryStatus.Open,
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            EntryType = EntryTypeData.GetData.ElementAt(3),
        },
        new(new Guid("10000000-0000-0000-0000-000000000006")) // 6
        {
            Notes = "Open WorkEntry assigned to inactive user.",
            Closed = false,
            Status = WorkEntryStatus.Open,
            ReceivedBy = UserData.GetUsers.ElementAt(3),
            EntryType = EntryTypeData.GetData.ElementAt(0),
        },
    };

    private static IEnumerable<WorkEntry>? _workEntries;

    public static IEnumerable<WorkEntry> GetData
    {
        get
        {
            if (_workEntries is not null) return _workEntries;

            _workEntries = WorkEntrySeedItems.ToList();
            _workEntries.ElementAt(3).SetDeleted("00000000-0000-0000-0000-000000000001");

            return _workEntries;
        }
    }

    public static void ClearData() => _workEntries = null;
}
