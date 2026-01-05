using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;

namespace AirWeb.TestData.Compliance;

internal static partial class ComplianceWork
{
    internal static IEnumerable<RmpInspection> RmpInspectionData =>
    [
        new(10001, (FacilityId)"00100001", UserData.GetRandomUser())
        {
            ComplianceWorkType = ComplianceWorkType.RmpInspection,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-10)),
            Notes = "In compliance",
            ClosedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-10)),

            InspectionReason = InspectionReason.PlannedUnannounced,
            InspectionStarted = DateTime.Now.AddYears(-5).AddDays(-10),
            InspectionEnded = DateTime.Now.AddYears(-5).AddDays(-10).AddHours(3),
            WeatherConditions = SampleText.GetRandomText(SampleText.TextLength.Phrase),
            InspectionGuide = SampleText.GetRandomText(SampleText.TextLength.Word),
            FacilityOperating = true,
            DeviationsNoted = false,
            FollowupTaken = false,
        },
        new(10002, DomainData.GetRandomFacility().Id, UserData.GetRandomUser())
        {
            ComplianceWorkType = ComplianceWorkType.RmpInspection,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTime.Today.AddYears(-2)),
            Notes = "Not in compliance",
            ClosedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2)),

            InspectionReason = InspectionReason.Complaint,
            InspectionStarted = DateTime.Now.AddYears(-3).AddDays(-1),
            InspectionEnded = DateTime.Now.AddYears(-3).AddDays(-1).AddHours(3),
            WeatherConditions = string.Empty,
            InspectionGuide = string.Empty,
            FacilityOperating = false,
            DeviationsNoted = true,
            FollowupTaken = true,
        },
        new(10003, DomainData.GetRandomFacility().Id)
        {
            ComplianceWorkType = ComplianceWorkType.RmpInspection,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted RMP Inspection",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            InspectionStarted = DateTime.Now.AddYears(-3).AddDays(-1),
            InspectionEnded = DateTime.Now.AddYears(-3).AddDays(-1).AddHours(3),
        },
    ];
}
