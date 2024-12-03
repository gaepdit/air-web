using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Compliance;

internal static partial class WorkEntries
{
    internal static IEnumerable<Report> ReportData =>
    [
        new(9001, UserData.GetUsers.ElementAt(0), DomainData.GetRandomFacility().Id)
        {
            WorkEntryType = WorkEntryType.Report,
            ResponsibleStaff = UserData.GetUsers.ElementAt(0),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-10).Date),
            Notes = "In compliance",
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-10)),

            ReceivedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-11).Date),
            EventDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-11).Date),
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
        new(9002, UserData.GetUsers.ElementAt(1), DomainData.GetRandomFacility().Id)
        {
            WorkEntryType = WorkEntryType.Report,
            ResponsibleStaff = UserData.GetUsers.ElementAt(1),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).Date),
            Notes = "Not in compliance",
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),

            ReceivedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
            EventDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
            ReportingPeriodType = ReportingPeriodType.Other,
            ReportingPeriodStart = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-21).Date),
            ReportingPeriodEnd = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-21).Date),
            ReportingPeriodComment = SampleText.GetRandomText(SampleText.TextLength.Word),
            DueDate = null,
            SentDate = null,
            ReportComplete = false,
            ReportsDeviations = true,
            EnforcementNeeded = true,
        },
        new(9003, null, DomainData.GetRandomFacility().Id)
        {
            WorkEntryType = WorkEntryType.Report,
            ResponsibleStaff = UserData.GetUsers.ElementAt(3),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted Report",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            ReceivedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
            EventDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
            ReportingPeriodType = ReportingPeriodType.Other,
            ReportingPeriodStart = new DateOnly(2020, 1, 1),
            ReportingPeriodEnd = new DateOnly(2020, 12, 31),
        },
    ];
}
