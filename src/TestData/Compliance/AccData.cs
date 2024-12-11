using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;

namespace AirWeb.TestData.Compliance;

internal static partial class WorkEntries
{
    internal static IEnumerable<AnnualComplianceCertification> AccData =>
    [
        new(5001, (FacilityId)"00100001")
        {
            WorkEntryType = WorkEntryType.AnnualComplianceCertification,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-10).Date),
            Notes = "Open ACC",

            ReceivedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-11).Date),
            EventDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-11).Date),
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
        new(5002, DomainData.GetRandomFacility().Id)
        {
            WorkEntryType = WorkEntryType.AnnualComplianceCertification,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-10).Date),
            Notes = "Closed ACC",
            ClosedBy = UserData.GetRandomUser(),
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-10)),

            ReceivedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
            EventDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
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
        new(5003, DomainData.GetRandomFacility().Id)
        {
        WorkEntryType = WorkEntryType.AnnualComplianceCertification,
        ResponsibleStaff = UserData.GetRandomUser(),
        AcknowledgmentLetterDate = null,
        Notes = "Deleted ACC",
        DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

        ReceivedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
        EventDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
        AccReportingYear = 2002,
        Postmarked = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-21).Date),
        },
    ];
}
