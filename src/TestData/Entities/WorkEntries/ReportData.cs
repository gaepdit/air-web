using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Entities.WorkEntries;

internal static partial class WorkEntries
{
    internal static IEnumerable<Report> ReportData =>
    [
        new(9001, UserData.GetUsers.ElementAt(0))
        {
            WorkEntryType = WorkEntryType.Report,
            Facility = DomainData.GetRandomFacility(),
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
            DueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-11).Date),
            SentDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-21).Date),
            ReportComplete = true,
            ReportsDeviations = false,
            EnforcementNeeded = false,
        },
        new(9002, UserData.GetUsers.ElementAt(1))
        {
            WorkEntryType = WorkEntryType.Report,
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(1),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).Date),
            Notes = "Not in compliance",
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),

            ReceivedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
            EventDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
            ReportingPeriodType = ReportingPeriodType.Other,
            ReportingPeriodStart = new DateOnly(2020, 1, 1),
            ReportingPeriodEnd = new DateOnly(2020, 12, 31),
            ReportingPeriodComment = SampleText.GetRandomText(SampleText.TextLength.Word),
            DueDate = null,
            SentDate = null,
            ReportComplete = false,
            ReportsDeviations = true,
            EnforcementNeeded = true,
        },
        new(9003, null)
        {
            WorkEntryType = WorkEntryType.Report,
            Facility = DomainData.GetRandomFacility(),
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
