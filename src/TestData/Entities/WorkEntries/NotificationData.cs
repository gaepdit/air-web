using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Entities.WorkEntries;

internal static partial class WorkEntries
{
    internal static IEnumerable<Notification> NotificationData =>
    [
        new(7001)
        {
            WorkEntryType = WorkEntryType.Notification,
            NotificationType = DomainData.GetRandomNotificationType(),
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(0),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-10).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
            ClosedBy = UserData.GetUsers.ElementAt(0),
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-4).AddDays(-10)),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-4).AddDays(-15)),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-4).AddDays(-15)),
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-4).AddDays(-12)),
            SentDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-4).AddDays(-20)),
            FollowupTaken = false,
        },
        new(7002)
        {
            WorkEntryType = WorkEntryType.Notification,
            NotificationType = DomainData.GetRandomNotificationType(),
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(1),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).Date),
            Notes = string.Empty,
            ClosedBy = UserData.GetUsers.ElementAt(1),
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-15)),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-15)),
            DueDate = null,
            SentDate = null,
            FollowupTaken = false,
        },
        new(7003)
        {
            WorkEntryType = WorkEntryType.Notification,
            NotificationType = DomainData.GetRandomNotificationType(),
            Facility = DomainData.GetRandomFacility(),
            ResponsibleStaff = UserData.GetUsers.ElementAt(3),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted Inspection",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-15)),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-15)),
        },
    ];
}
