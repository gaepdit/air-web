using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Compliance;

internal static partial class WorkEntries
{
    internal static IEnumerable<Notification> NotificationData =>
    [
        new(7001, UserData.GetRandomUser())
        {
            WorkEntryType = WorkEntryType.Notification,
            NotificationType = DomainData.GetRandomNotificationType(),
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-10).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-4).AddDays(-10)),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-4).AddDays(-15)),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-4).AddDays(-15)),
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-4).AddDays(-12)),
            SentDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-4).AddDays(-20)),
            FollowupTaken = false,
        },
        new(7002, UserData.GetRandomUser())
        {
            WorkEntryType = WorkEntryType.Notification,
            NotificationType = DomainData.GetRandomNotificationType(),
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).Date),
            Notes = string.Empty,
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-15)),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-15)),
            DueDate = null,
            SentDate = null,
            FollowupTaken = false,
        },
        new(7003, null)
        {
            WorkEntryType = WorkEntryType.Notification,
            NotificationType = DomainData.GetRandomNotificationType(),
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted Inspection",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-15)),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-15)),
        },
    ];
}
