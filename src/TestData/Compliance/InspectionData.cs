using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Compliance;

internal static partial class WorkEntries
{
    internal static IEnumerable<Inspection> InspectionData =>
    [
        new(6001, null)
        {
            WorkEntryType = WorkEntryType.Inspection,
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-10).Date),
            Notes = "In compliance",
            ClosedBy = UserData.GetRandomUser(),
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-10)),

            InspectionReason = InspectionReason.PlannedUnannounced,
            InspectionStarted = DateTime.Now.AddYears(-5).AddDays(-10),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-5).AddDays(-10)),
            InspectionEnded = DateTime.Now.AddYears(-5).AddDays(-10).AddHours(3),
            WeatherConditions = SampleText.GetRandomText(SampleText.TextLength.Phrase),
            InspectionGuide = SampleText.GetRandomText(SampleText.TextLength.Word),
            FacilityOperating = true,
            DeviationsNoted = false,
            FollowupTaken = false,
        },
        new(6002, null)
        {
            WorkEntryType = WorkEntryType.Inspection,
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).Date),
            Notes = "Not in compliance",
            ClosedBy = UserData.GetRandomUser(),
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),

            InspectionReason = InspectionReason.Complaint,
            InspectionStarted = DateTime.Now.AddYears(-3).AddDays(-1),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-1)),
            InspectionEnded = DateTime.Now.AddYears(-3).AddDays(-1).AddHours(3),
            WeatherConditions = string.Empty,
            InspectionGuide = string.Empty,
            FacilityOperating = false,
            DeviationsNoted = true,
            FollowupTaken = true,
        },
        new(6003, null)
        {
            WorkEntryType = WorkEntryType.Inspection,
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted Inspection",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            InspectionStarted = DateTime.Now.AddYears(-3).AddDays(-1),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-1)),
            InspectionEnded = DateTime.Now.AddYears(-3).AddDays(-1).AddHours(3),
        },
    ];
}
