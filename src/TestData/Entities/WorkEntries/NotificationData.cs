using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Entities.WorkEntries;

internal static partial class WorkEntries
{
    internal static IEnumerable<Notification> NotificationData =>
    [
        new Notification(7001)
        {
            WorkType = "Notification",
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
        new Notification(7002)
        {
            WorkType = "Notification",
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
        new Notification(7003)
        {
            WorkType = "Notification",
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
