using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using static AirWeb.TestData.Entities.WorkEntries.WorkEntries;

namespace AirWeb.TestData.Entities;

internal static class WorkEntryData
{
    private static IEnumerable<BaseWorkEntry> WorkEntrySeedItems
    {
        get
        {
            var entries = new List<BaseWorkEntry>();
            entries.AddRange(AccData);
            entries.AddRange(InspectionData);
            entries.AddRange(NotificationData);
            entries.AddRange(PermitRevocationData);
            entries.AddRange(ReportData);
            entries.AddRange(RmpInspectionData);
            entries.AddRange(SourceTestReviewData);
            return entries;
        }
    }

    private static IEnumerable<BaseWorkEntry>? _workEntries;

    public static IEnumerable<BaseWorkEntry> GetData
    {
        get
        {
            if (_workEntries is not null) return _workEntries;
            _workEntries = WorkEntrySeedItems.ToList();

            // Add comments
            foreach (var workEntry in _workEntries)
                workEntry.Comments.AddRange(CommentData.GetRandomCommentsList());

            // Set as deleted
            _workEntries.Single(entry => entry.Id == 5003).SetDeleted(UserData.AdminUserId);
            _workEntries.Single(entry => entry.Id == 6003).SetDeleted(UserData.AdminUserId);
            _workEntries.Single(entry => entry.Id == 7003).SetDeleted(UserData.AdminUserId);
            _workEntries.Single(entry => entry.Id == 8003).SetDeleted(UserData.AdminUserId);
            _workEntries.Single(entry => entry.Id == 9003).SetDeleted(UserData.AdminUserId);
            _workEntries.Single(entry => entry.Id == 10003).SetDeleted(UserData.AdminUserId);
            _workEntries.Single(entry => entry.Id == 11003).SetDeleted(UserData.AdminUserId);

            return _workEntries;
        }
    }

    public static void ClearData() => _workEntries = null;
}
