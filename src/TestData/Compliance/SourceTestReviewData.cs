using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;
using IaipDataService.TestData;

namespace AirWeb.TestData.Compliance;

internal static partial class ComplianceWork
{
    internal static IEnumerable<SourceTestReview> SourceTestReviewData =>
    [
        new(11001, SourceTestData.GetData[0].Facility!.Id, UserData.GetRandomUser())
        {
            WorkEntryType = WorkEntryType.SourceTestReview,
            ReferenceNumber = SourceTestData.GetData[0].ReferenceNumber,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-10)),
            Notes = "In compliance",
            ClosedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-10)),
            ReceivedByComplianceDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-20)),
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddMonths(-2)),
            FollowupTaken = false,
        },
        new(11002, SourceTestData.GetData[1].Facility!.Id, UserData.GetRandomUser())
        {
            WorkEntryType = WorkEntryType.SourceTestReview,
            ReferenceNumber = SourceTestData.GetData[1].ReferenceNumber,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTime.Today.AddYears(-2)),
            Notes = "Not in compliance",
            ClosedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2)),
            ReceivedByComplianceDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-20)),
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddMonths(-2)),
            FollowupTaken = true,
        },
        new(11003, SourceTestData.GetData[2].Facility!.Id)
        {
            WorkEntryType = WorkEntryType.SourceTestReview,
            ReferenceNumber = SourceTestData.GetData[2].ReferenceNumber,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted Source Test Review",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
            ReceivedByComplianceDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-20)),
        },
        new(11004, (FacilityId)"001-00001")
        {
            WorkEntryType = WorkEntryType.SourceTestReview,
            ReferenceNumber = null,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate = null,
            Notes = "Historical Source Test Review with no Reference Number",
            ClosedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-12)),
            ReceivedByComplianceDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-12).AddDays(-12)),
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-12).AddMonths(-12)),
            FollowupTaken = true,
        },
    ];
}
