using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.TestData.Constants;
using AirWeb.TestData.Identity;

namespace AirWeb.TestData;

internal static class WorkEntryData
{
    private static IEnumerable<BaseWorkEntry> WorkEntrySeedItems => new List<BaseWorkEntry>
    {
        new(0)
        {
            IsClosed = true,
            Status = WorkEntryStatus.Closed,
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            EntryType = EntryTypeData.GetData.ElementAt(0),
            Notes = TextData.Paragraph,
        },
        new(1)
        {
            IsClosed = false,
            Status = WorkEntryStatus.Open,
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            ReceivedDate = DateTimeOffset.Now.AddMinutes(30),
            EntryType = EntryTypeData.GetData.ElementAt(0),
        },
        new(2)
        {
            IsClosed = true,
            Status = WorkEntryStatus.Closed,
            ReceivedBy = UserData.GetUsers.ElementAt(2),
            EntryType = EntryTypeData.GetData.ElementAt(1),
        },
        new(3)
        {
            Notes = "Deleted work entry",
            IsClosed = true,
            Status = WorkEntryStatus.Closed,
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            DeleteComments = TextData.Paragraph,
            EntryType = EntryTypeData.GetData.ElementAt(2),
        },
        new(4)
        {
            IsClosed = false,
            Status = WorkEntryStatus.Open,
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            EntryType = null,
        },
        new(5)
        {
            IsClosed = false,
            Status = WorkEntryStatus.Open,
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            EntryType = EntryTypeData.GetData.ElementAt(3),
        },
        new(6)
        {
            Notes = "Open WorkEntry assigned to inactive user.",
            IsClosed = false,
            Status = WorkEntryStatus.Open,
            ReceivedBy = UserData.GetUsers.ElementAt(3),
            EntryType = EntryTypeData.GetData.ElementAt(0),
        },
    };

    private static IEnumerable<BaseWorkEntry>? _workEntries;

    public static IEnumerable<BaseWorkEntry> GetData
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
