using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.Actions;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Enforcement;

public static class EnforcementActionData
{
    private static IEnumerable<EnforcementAction> EnforcementActionSeedItems =>
    [
        // 301 (0)
        new LetterOfNoncompliance(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(1), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 302 (1)
        new LetterOfNoncompliance(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(2), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-10).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseRequested = true,
        },

        // 303 (2)
        new LetterOfNoncompliance(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(3), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-10).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseRequested = true,
            ResponseReceived = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-5).Date),
        },

        // 304 (3, 4)
        new LetterOfNoncompliance(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(4), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseRequested = true,
            ResponseReceived = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).Date),
            ClosedAsUnsent = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-5).Date),
        },
        new NoticeOfViolation(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(4), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 305 (5)
        new NoticeOfViolation(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(5), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-15).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 306 (6)
        new NoticeOfViolation(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(6), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-15).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseReceived = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-5).Date),
        },

        // 307 (7)
        new NovNfaLetter(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(7), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-75).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseReceived = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-65).Date),
        },

        // 308 (8)
        new NoticeOfViolation(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(8), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-15).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseReceived = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-5).Date),
        },

        // 309 (9)
        new ProposedConsentOrder(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(9), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-5).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 310 (10)
        new ProposedConsentOrder(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(10), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-5).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 311 (11)
        new ProposedConsentOrder(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(11), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-5).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 312 (12)
        new ProposedConsentOrder(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(12), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-5).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 313 (13)
        new AdministrativeOrder(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(13), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-88).Date),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            Executed = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-90).Date),
        },

        // 314 (14)
        new AdministrativeOrder(Guid.NewGuid(), EnforcementCaseData.GetData.ElementAt(14), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            Executed = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-220).Date),
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
            Executed = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-20).Date),
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
            Executed = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-20).Date),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-19).Date),
            OrderNumber = 1663,
            PenaltyAmount = 10_000,
            PenaltyComment = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            Resolved = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).Date),
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
                enforcementAction.CurrentOwner = UserData.GetRandomUser();
                enforcementAction.CurrentOwnerAssignedDate = DateTimeOffset.Now.AddDays(-10);
            }

            foreach (var enforcementAction in _enforcementActions.Where(action => action.IsIssued))
            {
                enforcementAction.IsApproved = true;
                enforcementAction.ApprovedBy = UserData.GetRandomUser();
                enforcementAction.DateApproved = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-5).Date);
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
        var random = new Random();
        for (var i = 0; i < 3; i++)
        {
            var penalty = new StipulatedPenalty(Guid.NewGuid(), consentOrder, UserData.GetRandomUser())
            {
                ConsentOrder = consentOrder,
                Amount = random.Next(1000, 5000),
                Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
                DateReceived = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddMonths(i + 1).Date),
            };
            consentOrder.StipulatedPenalties.Add(penalty);
        }
    }

    private static void GenerateEnforcementActionReviews(EnforcementAction enforcementAction)
    {
        var reviewCount = new Random().Next(1, 3); // Generate 1 or 2 reviews
        for (var i = 0; i < reviewCount; i++)
        {
            var review = new EnforcementActionReview(Guid.NewGuid(), enforcementAction, UserData.GetRandomUser())
            {
                DateRequested = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-10 * (i + 1)).Date),
                DateCompleted = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-5 * (i + 1)).Date),
                Status = (ReviewResult)new Random().Next(0, 4), // Random status
                ReviewComments = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
                ReviewedBy = UserData.GetRandomUser(),
            };
            enforcementAction.Reviews.Add(review);
        }
    }

    public static void ClearData() => _enforcementActions = null;
}
