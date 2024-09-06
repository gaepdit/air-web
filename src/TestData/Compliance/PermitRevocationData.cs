using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Compliance;

internal static partial class WorkEntries
{
    internal static IEnumerable<PermitRevocation> PermitRevocationData =>
    [
        new(8001)
        {
            WorkEntryType = WorkEntryType.PermitRevocation,
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(0),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-10).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            ReceivedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-11).Date),
            EventDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-11).Date),
            PermitRevocationDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-1).Date),
            PhysicalShutdownDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-21).Date),
            FollowupTaken = false,
        },
        new(8002)
        {
            WorkEntryType = WorkEntryType.PermitRevocation,
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(1),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-10).Date),
            Notes = string.Empty,
            IsClosed = true,
            ClosedBy = UserData.GetUsers.ElementAt(1),
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-10)),

            ReceivedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
            EventDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
            PermitRevocationDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-1).Date),
            PhysicalShutdownDate = null,
            FollowupTaken = true,
        },
        new(8003)
        {
            WorkEntryType = WorkEntryType.PermitRevocation,
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(3),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted permit revocation",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            ReceivedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
            EventDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-11).Date),
            PermitRevocationDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-1).Date),
        },
    ];
}
