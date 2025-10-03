using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;
using static AirWeb.TestData.Compliance.ComplianceWork;

namespace AirWeb.TestData.Compliance;

internal static class WorkEntryData
{
    private static IEnumerable<WorkEntry> WorkEntrySeedItems
    {
        get
        {
            var entries = new List<WorkEntry>();
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

    private static IEnumerable<WorkEntry>? _workEntries;

    public static ComplianceEvent? GetRandomComplianceEvent(FacilityId facilityId) =>
        (ComplianceEvent?)GetData
            .Where(entry => entry is ComplianceEvent && !entry.IsDeleted && entry.FacilityId == facilityId)
            .OrderBy(_ => Guid.NewGuid()).FirstOrDefault();

    public static IEnumerable<WorkEntry> GetData
    {
        get
        {
            if (_workEntries is not null) return _workEntries;
            _workEntries = WorkEntrySeedItems.ToList();

            // Add comments
            foreach (var workEntry in _workEntries)
            {
                // For testing purposes, different entry types will get different numbers of comments.
                switch (workEntry)
                {
                    case RmpInspection:
                        // Don't add comments to RMP Inspections.
                        continue;
                    case Notification:
                        // Add at least one comment to each Notification.
                        workEntry.Comments.AddRange(CommentData.GetRandomCommentsList(1)
                            .Select(comment => new WorkEntryComment(comment, workEntry.Id)));
                        break;
                    default:
                        // All others get zero or more comments.
                        workEntry.Comments.AddRange(CommentData.GetRandomCommentsList()
                            .Select(comment => new WorkEntryComment(comment, workEntry.Id)));
                        break;
                }
            }

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
