using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;

namespace AirWeb.TestData.Compliance;

internal static partial class ComplianceWork
{
    internal static IEnumerable<PermitRevocation> PermitRevocationData =>
    [
        new(8001, (FacilityId)"00100001")
        {
            WorkEntryType = WorkEntryType.PermitRevocation,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-10)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-11)),
            PermitRevocationDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-1)),
            PhysicalShutdownDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-21)),
            FollowupTaken = false,
        },
        new(8002, DomainData.GetRandomFacility().Id)
        {
            WorkEntryType = WorkEntryType.PermitRevocation,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-10)),
            Notes = string.Empty,
            ClosedBy = UserData.GetRandomUser(),
            ClosedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-10)),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-11)),
            PermitRevocationDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-1)),
            PhysicalShutdownDate = null,
            FollowupTaken = true,
        },
        new(8003, DomainData.GetRandomFacility().Id)
        {
            WorkEntryType = WorkEntryType.PermitRevocation,
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted permit revocation",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-11)),
            PermitRevocationDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-1)),
        },
    ];
}
