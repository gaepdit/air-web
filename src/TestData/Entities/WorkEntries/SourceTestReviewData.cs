using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Entities.WorkEntries;

internal static partial class WorkEntries
{
    internal static IEnumerable<SourceTestReview> SourceTestReviewData =>
    [
        new SourceTestReview(11001)
        {
            WorkEntryType = WorkEntryType.SourceTestReview,
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(0),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-10).Date),
            Notes = "In compliance",
            ClosedBy = UserData.GetUsers.ElementAt(0),
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-10)),

            ReferenceNumber = SampleText.ValidReferenceNumber,
            ReceivedByCompliance = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-20)),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-20)),
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddMonths(-2)),
            FollowupTaken = false,
        },
        new SourceTestReview(11002)
        {
            WorkEntryType = WorkEntryType.SourceTestReview,
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(1),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).Date),
            Notes = "Not in compliance",
            ClosedBy = UserData.GetUsers.ElementAt(1),
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),

            ReferenceNumber = SampleText.ValidReferenceNumber + 1,
            ReceivedByCompliance = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-20)),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-20)),
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddMonths(-2)),
            FollowupTaken = true,
        },
        new SourceTestReview(11003)
        {
            WorkEntryType = WorkEntryType.SourceTestReview,
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(3),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted Source Test Review",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            ReferenceNumber = SampleText.ValidReferenceNumber + 1,
            ReceivedByCompliance = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-20)),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-20)),
        },
    ];
}
