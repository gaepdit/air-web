using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.TestData.Constants;

namespace AirWeb.TestData;

internal static class WorkEntryData
{
    private static IEnumerable<BaseWorkEntry> WorkEntrySeedItems => new List<BaseWorkEntry>
    {
        new PermitRevocation(0)
        {
            IsClosed = true,
            Notes = TextData.Paragraph,
        },
        new PermitRevocation(1)
        {
            IsClosed = false,
            ReceivedDate =DateOnly.FromDateTime( DateTime.Now),
        },
        new PermitRevocation(2)
        {
            IsClosed = true,
        },
        new PermitRevocation(3)
        {
            Notes = "Deleted work entry",
            IsClosed = true,
            DeleteComments = TextData.Paragraph,
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
