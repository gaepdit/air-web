﻿using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;

namespace AirWeb.TestData.Compliance;

internal static partial class WorkEntries
{
    internal static IEnumerable<Notification> NotificationData =>
    [
        new(7001, (FacilityId)"00100001", UserData.GetRandomUser())
        {
            WorkEntryType = WorkEntryType.Notification,
            NotificationType = DomainData.GetRandomNotificationType(),
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-10)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
            ClosedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-10)),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-15)),
            EventDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-15)),
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-12)),
            SentDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-20)),
            FollowupTaken = false,
        },
        new(7002, DomainData.GetRandomFacility().Id, UserData.GetRandomUser())
        {
            WorkEntryType = WorkEntryType.Notification,
            NotificationType = DomainData.GetRandomNotificationType(),
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTime.Today.AddYears(-2)),
            Notes = string.Empty,
            ClosedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2)),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-15)),
            EventDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-15)),
            DueDate = null,
            SentDate = null,
            FollowupTaken = false,
        },
        new(7003, DomainData.GetRandomFacility().Id)
        {
            WorkEntryType = WorkEntryType.Notification,
            NotificationType = DomainData.GetRandomNotificationType(),
            ResponsibleStaff = UserData.GetRandomUser(),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted Inspection",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            ReceivedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-15)),
            EventDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-15)),
        },
    ];
}
