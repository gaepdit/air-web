using AirWeb.AppServices.Core.AppNotifications;

namespace AirWeb.AppServices.Compliance.AppNotifications;

public class EnforcementTemplate : Template
{
    // Enforcement email templates
    private const string CreatedBody = "Enforcement Case ID {0} has been started for Facility {1} by {2}.";

    public static readonly EnforcementTemplate EnforcementCreated = new()
    {
        Subject = "Enforcement Case Started",
        TextBody = CreatedBody,
        HtmlBody = $"<p>{CreatedBody}</p>",
    };

    private const string UpdatedBody = "Enforcement Case ID {0} has been updated by {1}.";

    public static readonly EnforcementTemplate EnforcementUpdated = new()
    {
        Subject = "Enforcement Case Updated",
        TextBody = UpdatedBody,
        HtmlBody = $"<p>{UpdatedBody}</p>",
    };

    public static readonly EnforcementTemplate EnforcementCommentAdded = new()
    {
        Subject = "New Comment on Enforcement Case",
        TextBody =
            """
            Enforcement Case ID {0} has a new comment by {2}:

            {1}

            """,
        HtmlBody = "<p>Enforcement Case ID {0} has a new comment by {2}:</p>" +
                   "<blockquote style='white-space:pre-line'>{1}</blockquote>",
    };

    private const string ClosedBody = "Enforcement Case ID {0} has been closed by {1}.";

    public static readonly EnforcementTemplate EnforcementClosed = new()
    {
        Subject = "Enforcement Case Closed",
        TextBody = ClosedBody,
        HtmlBody = $"<p>{ClosedBody}</p>",
    };

    private const string ReopenedBody = "Enforcement Case ID {0} has been reopened by {1}.";

    public static readonly EnforcementTemplate EnforcementReopened = new()
    {
        Subject = "Enforcement Case Reopened",
        TextBody = ReopenedBody,
        HtmlBody = $"<p>{ReopenedBody}</p>",
    };

    private const string DeletedBody = "Enforcement Case ID {0} has been deleted by {1}.";

    public static readonly EnforcementTemplate EnforcementDeleted = new()
    {
        Subject = "Enforcement Case Deleted",
        TextBody = DeletedBody,
        HtmlBody = $"<p>{DeletedBody}</p>",
    };

    private const string RestoredBody = "Enforcement Case ID {0} has been restored by {1}.";

    public static readonly EnforcementTemplate EnforcementRestored = new()
    {
        Subject = "Enforcement Case Restored",
        TextBody = RestoredBody,
        HtmlBody = $"<p>{RestoredBody}</p>",
    };

    private const string ActionAddedBody = "An Enforcement Action has been added to Case ID {0} by {1}.";

    public static readonly EnforcementTemplate EnforcementActionAdded = new()
    {
        Subject = "Enforcement Case Updated",
        TextBody = ActionAddedBody,
        HtmlBody = $"<p>{ActionAddedBody}</p>",
    };

    private const string ActionDeletedBody = "An Enforcement Action was deleted from Case ID {0} by {1}.";

    public static readonly EnforcementTemplate EnforcementActionDeleted = new()
    {
        Subject = "Enforcement Case Updated",
        TextBody = ActionDeletedBody,
        HtmlBody = $"<p>{ActionDeletedBody}</p>",
    };

    private const string ActionReviewRequestedBody =
        "Your review has been requested by {1} for an Enforcement Action on Case ID {0}.";

    public static readonly EnforcementTemplate EnforcementActionReviewRequested = new()
    {
        Subject = "Enforcement Action Review Requested",
        TextBody = ActionReviewRequestedBody,
        HtmlBody = $"<p>{ActionReviewRequestedBody}</p>",
    };

    private const string ActionReviewCompletedBody =
        "An Enforcement Action review has been completed on Case ID {0} by {1}.";

    public static readonly EnforcementTemplate EnforcementActionReviewCompleted = new()
    {
        Subject = "Enforcement Action Review Completed",
        TextBody = ActionReviewCompletedBody,
        HtmlBody = $"<p>{ActionReviewCompletedBody}</p>",
    };
}
