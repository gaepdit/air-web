using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;

namespace AirWeb.TestData.Compliance;

internal static partial class WorkEntries
{
    internal static IEnumerable<Report> ReportData =>
    [
        new(9001, (FacilityId)"00100001", UserData.GetRandomUser())
        {
            WorkEntryType = WorkEntryType.Report,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-10)),
            Notes = "In compliance",
            ClosedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-10)),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-11)),
            EventDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-11)),
            ReportingPeriodType = ReportingPeriodType.Annual,
            ReportingPeriodStart = new DateOnly(2020, 1, 1),
            ReportingPeriodEnd = new DateOnly(2020, 12, 31),
            ReportingPeriodComment = null,
            DueDate = new DateOnly(2021, 1, 31),
            SentDate = new DateOnly(2021, 1, 1),
            ReportComplete = true,
            ReportsDeviations = false,
            EnforcementNeeded = false,
        },
        new(9002, DomainData.GetRandomFacility().Id, UserData.GetRandomUser())
        {
            WorkEntryType = WorkEntryType.Report,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTime.Today.AddYears(-2)),
            Notes = "Not in compliance",
            ClosedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2)),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-11)),
            EventDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-11)),
            ReportingPeriodType = ReportingPeriodType.Other,
            ReportingPeriodStart = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-21)),
            ReportingPeriodEnd = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-21)),
            ReportingPeriodComment = SampleText.GetRandomText(SampleText.TextLength.Word),
            DueDate = null,
            SentDate = null,
            ReportComplete = false,
            ReportsDeviations = true,
            EnforcementNeeded = true,
        },
        new(9003, DomainData.GetRandomFacility().Id)
        {
            WorkEntryType = WorkEntryType.Report,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted Report",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-11)),
            EventDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-11)),
            ReportingPeriodType = ReportingPeriodType.Other,
            ReportingPeriodStart = new DateOnly(2020, 1, 1),
            ReportingPeriodEnd = new DateOnly(2020, 12, 31),
        },
    ];
}
