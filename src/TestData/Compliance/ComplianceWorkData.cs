using AirWeb.Domain.Comments;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;
using static AirWeb.TestData.Compliance.ComplianceMonitoringData;

namespace AirWeb.TestData.Compliance;

internal static class ComplianceWorkData
{
    private static IEnumerable<ComplianceWork> ComplianceWorkSeedItems
    {
        get
        {
            var entries = new List<ComplianceWork>();
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

    private static IEnumerable<ComplianceWork>? _complianceWork;

    public static ComplianceEvent? GetRandomComplianceEvent(FacilityId facilityId) =>
        (ComplianceEvent?)GetData
            .Where(work => work is ComplianceEvent && !work.IsDeleted && work.FacilityId == facilityId)
            .OrderBy(_ => Guid.NewGuid()).FirstOrDefault();

    public static IEnumerable<ComplianceWork> GetData
    {
        get
        {
            if (_complianceWork is not null) return _complianceWork;
            _complianceWork = ComplianceWorkSeedItems.ToList();

            // Add comments
            foreach (var complianceWork in _complianceWork)
            {
                // For testing purposes, different entry types will get different numbers of comments.
                switch (complianceWork)
                {
                    case RmpInspection:
                        // Don't add comments to RMP Inspections.
                        continue;
                    case Notification:
                        // Add at least one comment to each Notification.
                        complianceWork.Comments.AddRange(CommentData.GetRandomCommentsList(1)
                            .Select(comment => new ComplianceWorkComment(comment, complianceWork.Id)));
                        break;
                    default:
                        // All others get zero or more comments.
                        complianceWork.Comments.AddRange(CommentData.GetRandomCommentsList()
                            .Select(comment => new ComplianceWorkComment(comment, complianceWork.Id)));
                        break;
                }
            }

            // Set as deleted
            _complianceWork.Single(work => work.Id == 5003).SetDeleted(UserData.AdminUserId);
            _complianceWork.Single(work => work.Id == 6003).SetDeleted(UserData.AdminUserId);
            _complianceWork.Single(work => work.Id == 7003).SetDeleted(UserData.AdminUserId);
            _complianceWork.Single(work => work.Id == 8003).SetDeleted(UserData.AdminUserId);
            _complianceWork.Single(work => work.Id == 9003).SetDeleted(UserData.AdminUserId);
            _complianceWork.Single(work => work.Id == 10003).SetDeleted(UserData.AdminUserId);
            _complianceWork.Single(work => work.Id == 11003).SetDeleted(UserData.AdminUserId);

            return _complianceWork;
        }
    }

    public static void ClearData() => _complianceWork = null;
}
