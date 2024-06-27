using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Entities.WorkEntries;

internal static partial class AllWorkEntryData
{
    private static IEnumerable<AnnualComplianceCertification> AccData =>
    [
        new AnnualComplianceCertification(5001)
        {
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(0),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-10).Date),
            Notes = "Open ACC",

            ReceivedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-11).Date),
            AccReportingYear = 2000,
            Postmarked = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-21).Date),
            PostmarkedOnTime = true,
            SignedByRo = true,
            OnCorrectForms = true,
            IncludesAllTvConditions = true,
            CorrectlyCompleted = true,
            ReportsDeviations = false,
            IncludesPreviouslyUnreportedDeviations = true,
            ReportsAllKnownDeviations = true,
            ResubmittalRequired = false,
            EnforcementNeeded = false,
        },
        new AnnualComplianceCertification(5002)
        {
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(1),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-10).Date),
            Notes = "Closed ACC",
            IsClosed = true,
            ClosedBy = UserData.GetUsers.ElementAt(1),
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-10)),

            ReceivedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
            AccReportingYear = 2002,
            Postmarked = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-21).Date),
            PostmarkedOnTime = false,
            SignedByRo = false,
            OnCorrectForms = false,
            IncludesAllTvConditions = false,
            CorrectlyCompleted = false,
            ReportsDeviations = true,
            IncludesPreviouslyUnreportedDeviations = false,
            ReportsAllKnownDeviations = false,
            ResubmittalRequired = true,
            EnforcementNeeded = true,
        },
        new AnnualComplianceCertification(5003)
        {
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(3),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted ACC",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
        },
    ];
}
