﻿using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.Actions;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Enforcement;

public static class EnforcementActionData
{
    private static IEnumerable<EnforcementAction> EnforcementActionSeedItems =>
    [
        // 301 (0)
        new LetterOfNoncompliance(Guid.NewGuid(), CaseFileData.GetData.ElementAt(1), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 302 (1)
        new LetterOfNoncompliance(Guid.NewGuid(), CaseFileData.GetData.ElementAt(2), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-10).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseRequested = true,
        },

        // 303 (2)
        new LetterOfNoncompliance(Guid.NewGuid(), CaseFileData.GetData.ElementAt(3), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-10).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseRequested = true,
            ResponseReceived = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-5).Date),
        },

        // 304 (3, 4)
        new LetterOfNoncompliance(Guid.NewGuid(), CaseFileData.GetData.ElementAt(4), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseRequested = true,
            ResponseReceived = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).Date),
            ClosedAsUnsent = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-5).Date),
        },
        new NoticeOfViolation(Guid.NewGuid(), CaseFileData.GetData.ElementAt(4), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 305 (5)
        new NoticeOfViolation(Guid.NewGuid(), CaseFileData.GetData.ElementAt(5), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-15).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 306 (6)
        new NoticeOfViolation(Guid.NewGuid(), CaseFileData.GetData.ElementAt(6), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-15).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseReceived = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-5).Date),
        },

        // 307 (7)
        new NovNfaLetter(Guid.NewGuid(), CaseFileData.GetData.ElementAt(7), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-75).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseReceived = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-65).Date),
        },

        // 308 (8)
        new NoticeOfViolation(Guid.NewGuid(), CaseFileData.GetData.ElementAt(8), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-15).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseReceived = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-5).Date),
        },

        // 309 (9)
        new ProposedConsentOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(9), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-5).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 310 (10)
        new ProposedConsentOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(10), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-5).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 311 (11)
        new ProposedConsentOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(11), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-5).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 312 (12)
        new ProposedConsentOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(12), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-5).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 313 (13)
        new AdministrativeOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(13), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-88).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ExecutedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-90).Date),
        },

        // 314 (14)
        new AdministrativeOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(14), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ExecutedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-220).Date),
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-218).Date),
        },
    ];

    private static List<EnforcementAction> NestedSeedItems(List<EnforcementAction> parentActions) =>
    [
        // 306 (0) [15]
        new NoFurtherActionLetter(Guid.NewGuid(), (NoticeOfViolation)parentActions[6], null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-15).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 308 (1) [16]
        new ProposedConsentOrder(Guid.NewGuid(), (NoticeOfViolation)parentActions[8], null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 310 (2) [17]
        new ConsentOrder(Guid.NewGuid(), (ProposedConsentOrder)parentActions[10], null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ReceivedFromFacility = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-30).Date),
            PenaltyAmount = 1000,
            PenaltyComment = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 311 (3) [18]
        new ConsentOrder(Guid.NewGuid(), (ProposedConsentOrder)parentActions[11], null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ReceivedFromFacility = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-30).Date),
            ExecutedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-20).Date),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-19).Date),
            OrderNumber = 1552,
            PenaltyAmount = 1000,
            PenaltyComment = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 312 (4) [19]
        new ConsentOrder(Guid.NewGuid(), (ProposedConsentOrder)parentActions[12], null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-18).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ReceivedFromFacility = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-30).Date),
            ExecutedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-20).Date),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-19).Date),
            OrderNumber = 1663,
            PenaltyAmount = 10_000,
            PenaltyComment = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResolvedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).Date),
            StipulatedPenaltiesDefined = true,
        },

        // 314 (5) [20]
        new AoResolvedLetter(Guid.NewGuid(), (AdministrativeOrder)parentActions[14], null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-200).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },
    ];

    private static IEnumerable<EnforcementAction> DoubleNestedSeedItems(List<EnforcementAction> parentActions) =>
    [
        // 312 (0) [21]
        new CoResolvedLetter(Guid.NewGuid(), (ConsentOrder)parentActions[4], null)
        {
            Notes = "Deleted CO resolved letter",
        },
        // 312 (1) [22]
        new CoResolvedLetter(Guid.NewGuid(), (ConsentOrder)parentActions[4], null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(3).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },
    ];

    private static List<EnforcementAction>? _enforcementActions;

    public static IEnumerable<EnforcementAction> GetData
    {
        get
        {
            if (_enforcementActions is not null) return _enforcementActions;

            _enforcementActions = EnforcementActionSeedItems.ToList();
            var nestedSeedItems = NestedSeedItems(_enforcementActions);
            _enforcementActions.AddRange(nestedSeedItems);
            var doubleNestedItems = DoubleNestedSeedItems(nestedSeedItems);
            _enforcementActions.AddRange(doubleNestedItems);

            foreach (var enforcementAction in _enforcementActions)
            {
                enforcementAction.ResponsibleStaff = UserData.GetRandomUser();
                enforcementAction.CurrentReviewer = UserData.GetRandomUser();
                enforcementAction.ReviewRequestedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-10).Date);
            }

            foreach (var enforcementAction in _enforcementActions.Where(action => action.IsIssued))
            {
                enforcementAction.IsApproved = true;
                enforcementAction.ApprovedBy = UserData.GetRandomUser();
                enforcementAction.ApprovedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-5).Date);
                GenerateEnforcementActionReviews(enforcementAction);
            }

            GenerateStipulatedPenalties((ConsentOrder)_enforcementActions[19]);

            // Set as deleted
            _enforcementActions[21].SetDeleted(UserData.AdminUserId);

            return _enforcementActions;
        }
    }

    private static void GenerateStipulatedPenalties(ConsentOrder consentOrder)
    {
        for (var i = 0; i < 3; i++)
        {
            var penalty = new StipulatedPenalty(Guid.NewGuid(), consentOrder, UserData.GetRandomUser())
            {
                ConsentOrder = consentOrder,
                Amount = Random.Shared.Next(1000, 5000),
                Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
                ReceivedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddMonths(i + 1).Date),
            };
            consentOrder.StipulatedPenalties.Add(penalty);
        }
    }

    private static void GenerateEnforcementActionReviews(EnforcementAction enforcementAction)
    {
        var reviewCount = Random.Shared.Next(1, 4); // Generate 1 to 3 reviews.
        for (var i = 0; i < reviewCount; i++)
        {
            var incomplete = i + 1 == reviewCount && Random.Shared.NextBoolean(); // Final review may be incomplete.
            var review = GenerateReview(i, incomplete, enforcementAction);
            enforcementAction.Reviews.Add(review);
        }
    }

    private static EnforcementActionReview GenerateReview(int i, bool incomplete, EnforcementAction enforcementAction)
    {
        var requestedDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-10 * (i + 1));
        return new EnforcementActionReview(Guid.NewGuid(), enforcementAction, UserData.GetRandomUser(), null)
        {
            RequestedDate = requestedDate,
            CompletedDate = incomplete ? null : requestedDate.AddDays(Random.Shared.Next(1, 5)),
            Status = incomplete ? null : (ReviewResult)Random.Shared.Next(0, 4), // Random status
            ReviewComments = incomplete ? null : SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ReviewedBy = incomplete ? null : UserData.GetRandomUser(),
        };
    }

    // Next() returns an int in the range [0..Int32.MaxValue]
    private const int HalfMaxValue = int.MaxValue / 2;
    private static bool NextBoolean(this Random random) => random.Next() > HalfMaxValue;

    public static void ClearData() => _enforcementActions = null;
}
