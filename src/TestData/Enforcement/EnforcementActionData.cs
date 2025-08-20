using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
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
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-1).AddDays(-10)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseRequested = true,
        },

        // 303 (2)
        new LetterOfNoncompliance(Guid.NewGuid(), CaseFileData.GetData.ElementAt(3), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-10)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseRequested = true,
            ResponseReceived = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-5)),
        },

        // 304 (3)
        new LetterOfNoncompliance(Guid.NewGuid(), CaseFileData.GetData.ElementAt(4), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseRequested = true,
            CanceledDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-1).AddDays(-5)),
        },

        // 304 (4)
        new NoticeOfViolation(Guid.NewGuid(), CaseFileData.GetData.ElementAt(4), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 305 (5)
        new NoticeOfViolation(Guid.NewGuid(), CaseFileData.GetData.ElementAt(5), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-15)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 306 (6)
        new NoticeOfViolation(Guid.NewGuid(), CaseFileData.GetData.ElementAt(6), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-85)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseReceived = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-75)),
        },

        // 307 (7)
        new NovNfaLetter(Guid.NewGuid(), CaseFileData.GetData.ElementAt(7), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-75)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseReceived = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-65)),
        },

        // 308 (8)
        new NoticeOfViolation(Guid.NewGuid(), CaseFileData.GetData.ElementAt(8), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-15)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResponseReceived = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(5)),
        },

        // 309 (9)
        new ProposedConsentOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(9), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(50)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 310 (10)
        new ProposedConsentOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(10), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-5)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 311 (11)
        new ProposedConsentOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(11), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(145)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 312 (12)
        new ProposedConsentOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(12), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-195)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 313 (13)
        new AdministrativeOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(13), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-1).AddDays(-88)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-1).AddDays(-90)),
        },

        // 314 (14)
        new AdministrativeOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(14), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-300)),
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-288)),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-200)),
        },

        // 306 (15)
        new NoFurtherActionLetter(Guid.NewGuid(), CaseFileData.GetData.ElementAt(6), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-74)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 308 (16)
        new ProposedConsentOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(8), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 310 (17)
        new ConsentOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(10), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(20)),
            OrderId = 310,
            PenaltyAmount = 1000,
            PenaltyComment = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
        },

        // 311 (18)
        new ConsentOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(11), null)
        {
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(148)),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(160)),
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(161)),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(169)),
            OrderId = 311,
            PenaltyAmount = 1000,
            PenaltyComment = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            StipulatedPenaltiesDefined = true,
        },

        // 312 (19)
        new ConsentOrder(Guid.NewGuid(), CaseFileData.GetData.ElementAt(12), null)
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-180)),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-183)),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-180)),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-180)),
            OrderId = 312,
            PenaltyAmount = 10_000,
            PenaltyComment = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
        },

        // 302 (20)
        new LetterOfNoncompliance(Guid.NewGuid(), CaseFileData.GetData.ElementAt(2), null)
        {
            Notes = "Deleted LON",
            ResponseRequested = true,
        },
    ];

    private static List<EnforcementAction>? _enforcementActions;

    public static IEnumerable<EnforcementAction> GetData
    {
        get
        {
            if (_enforcementActions is not null) return _enforcementActions;

            _enforcementActions = EnforcementActionSeedItems.ToList();

            foreach (var action in _enforcementActions)
            {
                if (action.IsIssued)
                {
                    action.Status = EnforcementActionStatus.Issued;
                    action.ApprovedBy = UserData.GetRandomUser();
                    action.ApprovedDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-5));
                    GenerateEnforcementActionReviews(action);
                }
                else if (action.IsCanceled)
                {
                    action.Status = EnforcementActionStatus.Canceled;
                    GenerateEnforcementActionReviews(action);
                }
                else if (Random.Shared.NextBoolean())
                {
                    action.Status = EnforcementActionStatus.Approved;
                    action.ApprovedBy = UserData.GetRandomUser();
                    action.ApprovedDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-5));
                    GenerateEnforcementActionReviews(action);
                }
#pragma warning disable S1862 // Related "if/else if" statements should not have the same condition
                else if (Random.Shared.NextBoolean())
#pragma warning restore S1862
                {
                    action.Status = EnforcementActionStatus.ReviewRequested;
                    action.CurrentReviewer = UserData.GetRandomUser();
                    action.ReviewRequestedDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-3));
                }
                else
                {
                    action.Status = EnforcementActionStatus.Draft;
                }

                CaseFileData.GetData.Single(caseFile => caseFile.Id == action.CaseFile.Id)
                    .EnforcementActions.Add(action);
            }

            GenerateStipulatedPenalties((ConsentOrder)_enforcementActions[18]);

            // Set as deleted
            _enforcementActions[20].SetDeleted(UserData.AdminUserId);

            return _enforcementActions;
        }
    }

    private static void GenerateStipulatedPenalties(ConsentOrder consentOrder)
    {
        for (var i = 0; i < 3; i++)
        {
            var penalty = new StipulatedPenalty(Guid.NewGuid(), consentOrder,
                Random.Shared.Next(1000, 5000),
                DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddMonths(i + 1)),
                UserData.GetRandomUser())
            {
                ConsentOrder = consentOrder,
                Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
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
        var requestedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-10 * (i + 1));
        return new EnforcementActionReview(Guid.NewGuid(), enforcementAction, UserData.GetRandomUser(), null)
        {
            RequestedDate = requestedDate,
            CompletedDate = incomplete ? null : requestedDate.AddDays(Random.Shared.Next(1, 5)),
            Result = incomplete ? null : (ReviewResult)Random.Shared.Next(0, 4), // Random status
            ReviewComments = incomplete ? null : SampleText.GetRandomText(SampleText.TextLength.Paragraph, true),
            ReviewedBy = incomplete ? null : UserData.GetRandomUser(),
        };
    }

    // Next() returns an int in the range [0 to Int32.MaxValue]
    private const int HalfMaxValue = int.MaxValue / 2;
    private static bool NextBoolean(this Random random) => random.Next() > HalfMaxValue;

    public static void ClearData() => _enforcementActions = null;
}
