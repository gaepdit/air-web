using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;

namespace AirWeb.TestData.Compliance;

internal static partial class ComplianceWork
{
    internal static IEnumerable<Inspection> InspectionData =>
    [
        new(6001, (FacilityId)"00100001")
        {
            WorkEntryType = WorkEntryType.Inspection,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-10)),
            Notes = "In compliance",
            ClosedBy = UserData.GetRandomUser(),
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
        new(6002, DomainData.GetRandomFacility().Id)
        {
            WorkEntryType = WorkEntryType.Inspection,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTime.Today.AddYears(-2)),
            Notes = "Not in compliance",
            ClosedBy = UserData.GetRandomUser(),
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
        new(6003, DomainData.GetRandomFacility().Id)
        {
            WorkEntryType = WorkEntryType.Inspection,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted Inspection",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            InspectionStarted = DateTime.Now.AddYears(-3).AddDays(-1),
            InspectionEnded = DateTime.Now.AddYears(-3).AddDays(-1).AddHours(3),
        },
    ];
}
