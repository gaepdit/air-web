﻿using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.TestData;

namespace AirWeb.TestData.Compliance;

internal static partial class WorkEntries
{
    internal static IEnumerable<SourceTestReview> SourceTestReviewData =>
    [
        new(11001, UserData.GetUsers.ElementAt(0), FacilityData.GetFacility(SourceTestData.GetData[0].Facility!.Id).Id)
        {
            WorkEntryType = WorkEntryType.SourceTestReview,
            ReferenceNumber = SourceTestData.GetData[0].ReferenceNumber,
            ResponsibleStaff = UserData.GetUsers.ElementAt(0),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-10).Date),
            Notes = "In compliance",
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-10)),

            ReceivedByCompliance = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-20)),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddDays(-20)),
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-3).AddMonths(-2)),
            FollowupTaken = false,
        },
        new(11002, UserData.GetUsers.ElementAt(1), FacilityData.GetFacility(SourceTestData.GetData[0].Facility!.Id).Id)
        {
            WorkEntryType = WorkEntryType.SourceTestReview,
            ReferenceNumber = SourceTestData.GetData[1].ReferenceNumber,
            ResponsibleStaff = UserData.GetUsers.ElementAt(1),
            AcknowledgmentLetterDate =
                DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).Date),
            Notes = "Not in compliance",
            ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),

            ReceivedByCompliance = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-20)),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-20)),
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddMonths(-2)),
            FollowupTaken = true,
        },
        new(11003, null, FacilityData.GetFacility(SourceTestData.GetData[0].Facility!.Id).Id)
        {
            WorkEntryType = WorkEntryType.SourceTestReview,
            ReferenceNumber = SourceTestData.GetData[2].ReferenceNumber,
            ResponsibleStaff = UserData.GetUsers.ElementAt(3),
            AcknowledgmentLetterDate = null,
            Notes = "Deleted Source Test Review",
            DeleteComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph),

            ReceivedByCompliance = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-20)),
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2).AddDays(-20)),
        },
    ];
}
