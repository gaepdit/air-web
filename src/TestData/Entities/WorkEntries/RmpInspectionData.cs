using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Entities.WorkEntries;

internal static partial class WorkEntries
{
    internal static IEnumerable<RmpInspection> RmpInspectionData =>
    [
        new RmpInspection(10001)
        {
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(0),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-10).Date),
            Notes = "In compliance",
            ClosedBy = UserData.GetUsers.ElementAt(0),
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-10)),

            InspectionReason = InspectionReason.PlannedUnannounced,
            InspectionStarted = DateTime.Now.AddYears(-5).AddDays(-10),
            InspectionEnded = DateTime.Now.AddYears(-5).AddDays(-10).AddHours(3),
            WeatherConditions = SampleText.GetRandomText(SampleText.TextLength.Phrase),
            InspectionGuide = SampleText.GetRandomText(SampleText.TextLength.Word),
            FacilityOperating = true,
            ComplianceStatus = ComplianceStatus.InCompliance,
            FollowupTaken = false,
        },
        new RmpInspection(10002)
        {
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(1),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).Date),
            Notes = "Not in compliance",
            ClosedBy = UserData.GetUsers.ElementAt(1),
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),

            InspectionReason = InspectionReason.Complaint,
            InspectionStarted = DateTime.Now.AddYears(-3).AddDays(-1),
            InspectionEnded = DateTime.Now.AddYears(-3).AddDays(-1).AddHours(3),
            WeatherConditions = string.Empty,
            InspectionGuide = string.Empty,
            FacilityOperating = false,
            ComplianceStatus = ComplianceStatus.DeviationsNoted,
            FollowupTaken = true,
        },
        new RmpInspection(10003)
        {
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(3),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted RMP Inspection",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            InspectionStarted = DateTime.Now.AddYears(-3).AddDays(-1),
            InspectionEnded = DateTime.Now.AddYears(-3).AddDays(-1).AddHours(3),
        },
    ];
}
